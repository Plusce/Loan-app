using System.Collections.Generic;
using System.Threading.Tasks;
using LoanApp.Entities.Loan;
using LoanApp.Entities.Loan.Dto;

namespace LoanApp.Services
{
    public interface ILoanService
    {
        Task<Loan> Create(CreateLoanDto createLoanDto);

        Task<ICollection<UserLoanDto>> GetUserLoans(int userId);

        Task<LoanDto> Get(int loanId);

        Task<LoanCashTransfer> TransferCash(CreateLoanCashTransferDto createLoanCashTransferDto);
    }
}