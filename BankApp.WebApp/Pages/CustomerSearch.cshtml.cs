
using Microsoft.AspNetCore.Mvc.RazorPages;

using BankApp.Services.ViewModels;
using Microsoft.AspNetCore.Mvc;
using BankApp.Utilities;
using BankApp.Services; // Ensure the namespace where PagedResult is defined is included
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BankApp.Pages;

[Authorize(Roles = "Cashier")]
public class CustomerSearchModel : PageModel
{
    private readonly ICustomerService _customerService;
    private readonly ILogger<CustomerSearchModel> _logger;

    public CustomerSearchModel(ICustomerService customerService, ILogger<CustomerSearchModel> logger)
    {
        _customerService = customerService;
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

        Customers = await _customerService.SearchCustomer(searchTerm, pageIndex: 1);
    }
}
