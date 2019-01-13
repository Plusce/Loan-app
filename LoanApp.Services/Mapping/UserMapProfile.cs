using System;
using AutoMapper;
using LoanApp.Entities.Base;
using LoanApp.Entities.Base.Dto;

namespace LoanApp.Services.Mapping
{
    public class UserMapProfile : Profile
    {
        public UserMapProfile()
        {
            CreateMap<CreateUserDto, User>().AfterMap((src, dest) =>
            {
                dest.CreatedDate = DateTime.UtcNow;
            });
        }
    }
}