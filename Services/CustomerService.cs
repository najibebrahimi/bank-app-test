

using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Services.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace Services
{
    public class CustomerService
    {
        private readonly BankAppDataContext _context;

        public CustomerService(BankAppDataContext context)
        {
            _context = context;
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