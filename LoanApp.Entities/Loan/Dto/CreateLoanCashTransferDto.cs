using System.ComponentModel.DataAnnotations;

namespace LoanApp.Entities.Loan.Dto
{
    public class CreateLoanCashTransferDto
    {
        public CreateLoanCashTransferDto()
        {
            
        }

        public CreateLoanCashTransferDto(int loanId, ulong amount, LoanTransferType transferType)
        {
            LoanId = loanId;
            Amount = amount;
            TransferType = transferType;
        }

        [Required]
        public int LoanId { get; set; }

        [Required]
        public ulong Amount { get; set; }

        [Required]
        public LoanTransferType TransferType { get; set; }
    }
}