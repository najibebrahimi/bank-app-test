

using BankApp.Utilities;
using BankApp.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using BankApp.Services.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BankApp.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly BankAppDataContext _context;

        public CustomerService(BankAppDataContext context)
        {
            _context = context;
        }
        public async Task<PagedResult<CustomerViewModel>> SearchCustomerByNumberAsync(int customerNumber, int pageIndex)
        {
            var query = _context.Customers.AsQueryable();
            query = query.Where(c => c.CustomerId == customerNumber);

            return await PagedResult<CustomerViewModel>.CreateAsync(
                query.Select(c => new CustomerViewModel
                {
                    CustomerId = c.CustomerId,
                    PersonalNumber = c.NationalId,
                    Name = c.Givenname + " " + c.Surname,
                    Address = c.Streetaddress,
                    City = c.City
                }),
                pageIndex,
                50 // Items per page
            );
        }

        public async Task<PagedResult<CustomerViewModel>> SearchCustomer(string searchTerm, int pageIndex)
        {
            if (!string.IsNullOrEmpty(searchTerm))
            {
                if (int.TryParse(searchTerm, out int customerNumber))
                    return await SearchCustomerByNumberAsync(customerNumber, pageIndex);
                else
                    return await SearchCustomerByNameSurnameOrCityAsync(searchTerm, pageIndex);
            };

            return null;
        }

        public async Task<PagedResult<CustomerViewModel>> SearchCustomerByNameSurnameOrCityAsync(string searchTerm, int pageIndex)
        {
            var query = _context.Customers.AsQueryable();
            query = query.Where(c =>
                c.Givenname.Contains(searchTerm) ||
                c.Surname.Contains(searchTerm) ||
                c.City.Contains(searchTerm));

            return await PagedResult<CustomerViewModel>.CreateAsync(
                query.Select(c => new CustomerViewModel
                {
                    CustomerId = c.CustomerId,
                    PersonalNumber = c.NationalId,
                    Name = c.Givenname + " " + c.Surname,
                    Address = c.Streetaddress,
                    City = c.City
                }),
                pageIndex,
                50 // Items per page
            );
        }

        public async Task<CustomerDetailsViewModel> GetCustomerDetailsAsync(int customerId)
        {
            var customer = await _context.Customers
                                         .Include(c => c.Dispositions)
                                         .ThenInclude(d => d.Account)
                                         .FirstOrDefaultAsync(c => c.CustomerId == customerId);

            if (customer == null)
            {
                return null;
            }

            var customerDetails = new CustomerDetailsViewModel
            {
                CustomerId = customer.CustomerId,
                PersonalNumber = customer.NationalId,
                Name = $"{customer.Givenname} {customer.Surname}",
                Address = customer.Streetaddress,
                City = customer.City,
                Accounts = customer.Dispositions.Select(d => new AccountViewModel
                {
                    AccountId = d.Account.AccountId,
                    Balance = d.Account.Balance
                }).ToList(),
                TotalBalance = customer.Dispositions.Sum(d => d.Account.Balance)
            };

            return customerDetails;
        }
    }
}