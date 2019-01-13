using System;
using System.Threading.Tasks;
using LoanApp.Entities.Loan;
using LoanApp.Entities.Loan.Dto;

namespace LoanApp.Services.Validation
{
    public class LoanValidationService : ILoanValidationService
    {
        private readonly IUserService _userService;

        public LoanValidationService(IUserService userService)
        {
            _userService = userService;
        }

        public void Validate(CreateLoanCashTransferDto createLoanCashTransferDto, Loan loan)
        {
            if (!loan.IsActive)
            {
                throw new InvalidOperationException("There is no possible to make cash transfer when loan is settled already.");
            }
            if (loan.Lender == null || loan.Borrower == null)
            {
                throw new ArgumentException("At least one of the loan participants not found.");
            }
            if (createLoanCashTransferDto.TransferType == LoanTransferType.Supplement && 
                loan.Lender.Balance - (long)createLoanCashTransferDto.Amount < 0)
            {
                throw new InvalidOperationException("There is no possible to lend more money than lender contains.");
            }
            if (createLoanCashTransferDto.TransferType == LoanTransferType.Repayment &&
                loan.Borrower.Balance - (long)createLoanCashTransferDto.Amount < 0)
            {
                throw new InvalidOperationException("There is no possible to repay more money than borrower contains.");
            }
        }

        public async Task Validate(CreateLoanDto createLoanDto)
        {
            if (createLoanDto.BorrowerId == createLoanDto.LenderId)
            {
                throw new ArgumentException("Single person cannot be borrower and lender at one time.");
            }

            var lender = await _userService.Get(createLoanDto.LenderId);
            var borrower = await _userService.Get(createLoanDto.BorrowerId);
            if (lender == null || borrower == null)
            {
                throw new ArgumentException("At least one of the loan participants not found.");
            }
            if (lender.Balance - (long)createLoanDto.StartAmount < 0)
            {
                throw new InvalidOperationException("There is no possible to lend more money than lender contains.");
            }
        }

        public async Task Validate(int userId)
        {
            if (await _userService.Get(userId) == null)
            {
                throw new ArgumentException("User not found.");
            }
        }
    }
}