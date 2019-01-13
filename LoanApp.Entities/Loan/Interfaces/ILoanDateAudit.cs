using System;
using LoanApp.Entities.Base.Interfaces;

namespace LoanApp.Entities.Loan.Interfaces
{
    public interface ILoanDateAudit : ICreatedDateAudit
    {
        DateTime? RepaymentDate { get; set; }
    }
}