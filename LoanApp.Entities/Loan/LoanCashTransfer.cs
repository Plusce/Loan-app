using System;
using System.ComponentModel.DataAnnotations.Schema;
using LoanApp.Entities.Base.Interfaces;

namespace LoanApp.Entities.Loan
{
    public class LoanCashTransfer : ICreatedDateAudit
    {
        public int Id { get; set; }

        [ForeignKey(nameof(Loan))]
        public int LoanId { get; set; }

        public ulong Amount { get; set; }

        public LoanTransferType TransferType { get; set; }

        public DateTime CreatedDate { get; set; }

        public Loan Loan { get; set; }
    }
}