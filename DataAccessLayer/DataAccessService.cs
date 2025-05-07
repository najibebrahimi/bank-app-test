//------------------
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class DataAccessService
    {
        private readonly BankAppDataContext _context;
        private readonly ILogger<DataAccessService> _logger;

        public DataAccessService(BankAppDataContext context, ILogger<DataAccessService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Gets the customer query (for advanced filtering & pagination).
        /// </summary>
        public IQueryable<Customer> GetCustomersQuery()
        {
            return _context.Customers.AsNoTracking();
        }

        /// <summary>
        /// Retrieves a customer by ID asynchronously.
        /// </summary>
        public async Task<Customer> GetCustomerByIdAsync(int customerId)
        {
            try
            {
                return await _context.Customers.AsNoTracking()
                    .FirstOrDefaultAsync(c => c.CustomerId == customerId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving customer with ID {CustomerId}", customerId);
                return null;
            }
        }

        /// <summary>
        /// Adds a new customer asynchronously.
        /// </summary>
        public async Task<bool> AddCustomerAsync(Customer customer)
        {
            try
            {
                await _context.Customers.AddAsync(customer);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding a new customer.");
                return false;
            }
        }

        /// <summary>
        /// Updates an existing customer asynchronously.
        /// </summary>
        public async Task<bool> UpdateCustomerAsync(Customer customer)
        {
            try
            {
                _context.Customers.Update(customer);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating customer with ID {CustomerId}", customer.CustomerId);
                return false;
            }
        }

        /// <summary>
        /// Deletes a customer asynchronously.
        /// </summary>
        public async Task<bool> DeleteCustomerAsync(int customerId)
        {
            try
            {
                var customer = await _context.Customers.FindAsync(customerId);
                if (customer != null)
                {
                    _context.Customers.Remove(customer);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting customer with ID {CustomerId}", customerId);
                return false;
            }
        }
    }
}
