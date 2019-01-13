using System;
using System.ComponentModel.DataAnnotations.Schema;
using LoanApp.Entities.Base;
using LoanApp.Entities.Loan.Interfaces;

namespace LoanApp.Entities.Loan
{
    public class Loan : ILoanDateAudit
    {
        public int Id { get; set; }

        [ForeignKey(nameof(User))]
        public int LenderId { get; set; }

        [ForeignKey(nameof(User))]
        public int BorrowerId { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; }

        public DateTime? RepaymentDate { get; set; }

        public User Borrower { get; set; }

        public User Lender { get; set; }
    }
}