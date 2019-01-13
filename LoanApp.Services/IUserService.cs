using System.Collections.Generic;
using System.Threading.Tasks;
using LoanApp.Entities.Base;
using LoanApp.Entities.Base.Dto;

namespace LoanApp.Services
{
    public interface IUserService
    {
        Task<User> Create(CreateUserDto createUserDto);

        Task<IEnumerable<User>> GetAll();

        Task<IEnumerable<User>> GetAllBorrowers();

        Task<IEnumerable<User>> GetAllLenders();

        Task<User> Get(int userId);

        Task TransferCashToUser(int userId, ulong amount);
    }
}