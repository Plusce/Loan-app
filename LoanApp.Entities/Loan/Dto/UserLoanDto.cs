using System;

namespace LoanApp.Entities.Loan.Dto
{
    public class UserLoanDto
    {
        public int LoanId { get; set; }

        public int ContractedUserId { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; }

        public DateTime? RepaymentDate { get; set; }

        public string UserRole { get; set; }

        public ulong RemainingPayments { get; set; }
    }
}