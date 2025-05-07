
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using DataAccessLayer.Models;
using System.Threading.Tasks;
using System.Linq;
using Services.ViewModels;
using Microsoft.AspNetCore.Mvc;
using BankApp.Utilities; // Ensure the namespace where PagedResult is defined is included

namespace BankApp.Pages
{
    public class CustomerSearchModel : PageModel
    {
        private readonly BankAppDataContext _context;
        private readonly ILogger<CustomerSearchModel> _logger;

        public CustomerSearchModel(BankAppDataContext context, ILogger<CustomerSearchModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Search term entered by the user
        [BindProperty(SupportsGet = true)] // Allows binding to query string
        public string SearchTerm { get; set; }

        // Paginated list of customers
        public PagedResult<CustomerViewModel> Customers { get; set; }

        // Method executed on GET request
        public async Task OnGetAsync(string searchTerm, int pageIndex = 1)
        {
            SearchTerm = searchTerm;

            _logger.LogInformation($"Searching for customers with term: {searchTerm}");

            // Query to get customers from the database
            var query = _context.Customers.AsQueryable();

            // Apply search filters
            if (!string.IsNullOrEmpty(searchTerm))
            {
                if (int.TryParse(searchTerm, out int customerNumber))
                {
                    query = query.Where(c => c.CustomerId == customerNumber);
                }
                else
                {
                    query = query.Where(c =>
                        c.Givenname.Contains(searchTerm) ||
                        c.Surname.Contains(searchTerm) ||
                        c.City.Contains(searchTerm));
                }
            }

            // Create paginated results
            Customers = await PagedResult<CustomerViewModel>.CreateAsync(
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
    }
}
