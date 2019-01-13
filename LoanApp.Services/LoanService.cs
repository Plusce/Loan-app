using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using LoanApp.DAL;
using LoanApp.Entities.Base;
using LoanApp.Entities.Loan;
using LoanApp.Entities.Loan.Dto;
using LoanApp.Services.Mapping;
using LoanApp.Services.Validation;
using Microsoft.EntityFrameworkCore;

namespace LoanApp.Services
{
    public class LoanService : ILoanService
    {
        private readonly LoanContext _db;
        private readonly ILoanValidationService _loanValidationService;

        public LoanService(LoanContext db, ILoanValidationService loanValidationService)
        {
            _db = db;
            _loanValidationService = loanValidationService;
        }

        public async Task<Loan> Create(CreateLoanDto createLoanDto)
        {
            await _loanValidationService.Validate(createLoanDto);
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var loan = await _db.Loans.AddAsync(createLoanDto.MapTo<Loan>());
                    await _db.SaveChangesAsync();

                    await TransferCash(new CreateLoanCashTransferDto(loan.Entity.Id,
                        createLoanDto.StartAmount, LoanTransferType.Supplement));
                    transaction.Commit();

                    var loanDto = loan.Entity.MapTo<LoanDto>();
                    loanDto.RemainingPayments = createLoanDto.StartAmount;
                    return loanDto;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task<ICollection<UserLoanDto>> GetUserLoans(int userId)
        {
            await _loanValidationService.Validate(userId);
            var userLoans = _db.Loans.Where(l => l.BorrowerId == userId || l.LenderId == userId);

            var userLoanDtos = new Collection<UserLoanDto>();
            if (userLoans.Any())
            {
                foreach (var loan in userLoans)
                {
                    var loanDto = loan.MapTo<UserLoanDto>();
                    loanDto.UserRole = GetUserRole(loan);
                    loanDto.RemainingPayments = SumToRepay(loan.Id);
                    loanDto.ContractedUserId = GetContractedUserId(loan);
                    userLoanDtos.Add(loanDto);
                }
            }

            return userLoanDtos;

            string GetUserRole(Loan loan)
            {
                return loan.LenderId == userId ? "Lender" : "Borrower";
            }

            int GetContractedUserId(Loan loan)
            {
                return loan.LenderId == userId ? loan.BorrowerId : loan.LenderId;
            }
        }

        public Task<LoanDto> Get(int loanId)
        {
            var loanDto = _db.Loans
                .Include(l => l.Borrower)
                .Include(l => l.Lender)
                .FirstOrDefault(l => l.Id == loanId).MapTo<LoanDto>();

            loanDto.RemainingPayments = SumLoanValuesByTransferType(loanDto.Id, LoanTransferType.Supplement)
                -SumLoanValuesByTransferType(loanDto.Id, LoanTransferType.Repayment);

            if (loanDto == null)
            {
                throw new ArgumentException($"Loan with id {loanId} not found.");
            }
            return Task.FromResult(loanDto);
        }

        public async Task<LoanCashTransfer> TransferCash(CreateLoanCashTransferDto createLoanCashTransferDto)
        {
            var loanDto = await Get(createLoanCashTransferDto.LoanId);
            _loanValidationService.Validate(createLoanCashTransferDto, loanDto);

            if (createLoanCashTransferDto.TransferType == LoanTransferType.Repayment)
            {
                if (IsSurplusOrRepaid(createLoanCashTransferDto.Amount, SumToRepay(loanDto.Id)))
                {
                    createLoanCashTransferDto.Amount = SumToRepay(loanDto.Id);
                    var loan = loanDto.MapTo<Loan>();
                    loan.IsActive = false;
                    loan.RepaymentDate = DateTime.UtcNow;
                    DetachAllEntities();
                    _db.Loans.Update(loan);
                }
            }

            MakeCashTransferBetweenUsers(createLoanCashTransferDto, loanDto.Lender, loanDto.Borrower);
            var loanCashTransfer = createLoanCashTransferDto.MapTo<LoanCashTransfer>();
            await _db.LoanCashTransfers.AddAsync(loanCashTransfer);
            await _db.SaveChangesAsync();
            return loanCashTransfer;
        }

        private bool IsSurplusOrRepaid(ulong currentTransferAmount, ulong remainingPaymentsAmount)
        {
            return currentTransferAmount >= remainingPaymentsAmount;
        }

        private ulong SumToRepay(int loanId)
        {
            return SumLoanValuesByTransferType(loanId, LoanTransferType.Supplement) - 
                   SumLoanValuesByTransferType(loanId, LoanTransferType.Repayment);
        }

        private ulong SumLoanValuesByTransferType(int loanId, LoanTransferType transferType)
        {
            return (ulong)_db.LoanCashTransfers
                .Where(l => l.LoanId == loanId && l.TransferType == transferType)
                .Sum(l => (float)l.Amount);
        }

        private void MakeCashTransferBetweenUsers(CreateLoanCashTransferDto cashTransferDto, User lender, User borrower)
        {
            if (cashTransferDto.TransferType == LoanTransferType.Supplement)
            {
                lender.Balance = lender.Balance - (long)cashTransferDto.Amount;
                borrower.Balance = borrower.Balance + (long)cashTransferDto.Amount;
            }
            else
            {
                lender.Balance = lender.Balance + (long)cashTransferDto.Amount;
                borrower.Balance = borrower.Balance - (long)cashTransferDto.Amount;
            }
        }

        private void DetachAllEntities()
        {
            var changedEntriesCopy = _db.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added ||
                            e.State == EntityState.Modified ||
                            e.State == EntityState.Deleted ||
                            e.State == EntityState.Unchanged)
                .ToList();

            foreach (var entry in changedEntriesCopy)
                entry.State = EntityState.Detached;
        }
    }
}