

using BankApp.Utilities;
using BankApp.Services.ViewModels;

namespace BankApp.Services
{
    public interface ICustomerService
    {
        Task<PagedResult<CustomerViewModel>> SearchCustomerByNumberAsync(int customerNumber, int pageIndex);
        Task<PagedResult<CustomerViewModel>> SearchCustomer(string searchTerm, int pageIndex);
        Task<PagedResult<CustomerViewModel>> SearchCustomerByNameSurnameOrCityAsync(string searchTerm, int pageIndex);
        Task<CustomerDetailsViewModel> GetCustomerDetailsAsync(int customerId);
    }
}