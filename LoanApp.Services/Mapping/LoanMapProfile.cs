using System;
using AutoMapper;
using LoanApp.Entities.Loan;
using LoanApp.Entities.Loan.Dto;

namespace LoanApp.Services.Mapping
{
    public class LoanMapProfile : Profile
    {
        public LoanMapProfile()
        {
            CreateMap<CreateLoanDto, Loan>()
                .AfterMap((src, dest) =>
            {
                dest.CreatedDate = DateTime.UtcNow;
            });

            CreateMap<CreateLoanCashTransferDto, LoanCashTransfer>()
                .AfterMap((src, dest) =>
                {
                    dest.CreatedDate = DateTime.UtcNow;
                });

            CreateMap<Loan, LoanDto>().ReverseMap();
            CreateMap<Loan, UserLoanDto>().AfterMap((src, dest) => { dest.LoanId = src.Id; });
        }
    }
}