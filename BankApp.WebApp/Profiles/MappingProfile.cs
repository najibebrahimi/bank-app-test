using AutoMapper;
using BankApp.DataAccessLayer.Models;
using BankApp.Services.ViewModels;

namespace BankApp.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Customer Mapping
            CreateMap<Customer, CustomerViewModel>();

            // Account Mapping
            CreateMap<Account, AccountViewModel>();
        }

    }
}
