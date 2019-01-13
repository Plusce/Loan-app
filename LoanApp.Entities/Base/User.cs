using System;
using LoanApp.Entities.Base.Interfaces;

namespace LoanApp.Entities.Base
{
    public class User : ICreatedDateAudit
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public long Balance { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}