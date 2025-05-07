using AutoMapper;
using DataAccessLayer.Models;
using DataAccessLayer.ViewModels;

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
