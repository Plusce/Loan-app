namespace LoanApp.Entities.Loan.Dto
{
    public class LoanDto : Loan
    {
        public ulong RemainingPayments { get; set; }        
    }
}