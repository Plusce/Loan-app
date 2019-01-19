using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoanApp.DAL;
using LoanApp.Entities.Base;
using LoanApp.Entities.Base.Dto;
using LoanApp.Services.Mapping;
using Microsoft.EntityFrameworkCore;

namespace LoanApp.Services
{
    public class UserService : IUserService
    {
        private readonly LoanContext _db;

        public UserService(LoanContext db)
        {
            _db = db;

        }

        public async Task<User> Create(CreateUserDto createUserDto)
        {
            var user = createUserDto.MapTo<User>();
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
            return user;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _db.Users.ToListAsync();
        }

        public async Task<IEnumerable<User>> GetAllBorrowers()
        {
            return await (from users in _db.Users
                join loans in _db.Loans on users.Id equals loans.BorrowerId
                    select users).ToListAsync();
        }

        public async Task<IEnumerable<User>> GetAllLenders()
        {
            return await (from users in _db.Users
                join loans in _db.Loans on users.Id equals loans.LenderId
                select users).ToListAsync();
        }

        public Task<User> Get(int userId)
        {
            var user = _db.Users.Find(userId);
            if (user == null)
            {
                throw new ArgumentException($"User with id {userId} not found.");
            }
            return Task.FromResult(user);
        }

        public async Task TransferCashToUser(int userId, ulong amount)
        {
            var user = await Get(userId);

            if (user == null)
            {
                throw new ArgumentException($"User with id {userId} not found.", nameof(userId));
            }
            if (amount == 0)
            {
                throw new InvalidOperationException("There is no possible to make transfer without cash.");
            }

            user.Balance += (long)amount;
            await _db.SaveChangesAsync();
        }
    }
}