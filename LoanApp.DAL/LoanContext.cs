using LoanApp.Entities.Base;
using LoanApp.Entities.Loan;
using Microsoft.EntityFrameworkCore;

namespace LoanApp.DAL
{
    public class LoanContext : DbContext
    {
        public LoanContext(DbContextOptions<LoanContext> options)
            : base(options)
        { }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Loan> Loans { get; set; }

        public virtual DbSet<LoanCashTransfer> LoanCashTransfers { get; set; }
    }
}