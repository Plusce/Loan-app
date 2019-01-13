using System.Threading.Tasks;
using LoanApp.Entities.Loan;
using LoanApp.Entities.Loan.Dto;

namespace LoanApp.Services.Validation
{
    public interface ILoanValidationService
    {
        void Validate(CreateLoanCashTransferDto createLoanCashTransferDto, Loan loan);

        Task Validate(CreateLoanDto createLoanDto);

        Task Validate(int userId);
    }
}