using System.ComponentModel.DataAnnotations;

namespace LoanApp.Entities.Base.Dto
{
    public class CreateUserDto
    {
        //[Required]
        public string FullName { get; set; }

        //[Required]
        public ulong Balance { get; set; }
    }
}