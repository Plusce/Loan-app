
using System.ComponentModel.DataAnnotations;

namespace LoanApp.Entities.Loan.Dto
{
    public class CreateLoanDto
    {
        [Required]
        public int LenderId { get; set; }

        [Required]
        public int BorrowerId { get; set; }

        [Required]
        public ulong StartAmount { get; set; }
    }
}