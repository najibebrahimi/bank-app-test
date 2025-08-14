
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BankApp.DataAccessLayer.Models;
using System.Threading.Tasks;
using System.Linq;
using BankApp.Services;
using BankApp.Services.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace BankApp.Pages
{
    [Authorize(Roles = "Cashier")]
    public class CustomerDetailsModel : PageModel
    {
        private readonly ICustomerService _customerService;

        public CustomerDetailsModel(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public Services.ViewModels.CustomerDetailsViewModel CustomerDetails { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            CustomerDetails = await _customerService.GetCustomerDetailsAsync(id);

            if (CustomerDetails == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}