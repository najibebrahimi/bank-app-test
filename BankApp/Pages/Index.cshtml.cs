
using DataAccessLayer.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Services;
using System.Threading.Tasks;

namespace BankApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly TransactionService _transactionService;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(TransactionService transactionService, ILogger<IndexModel> logger)
        {
            _transactionService = transactionService;
            _logger = logger;
        }

        public Statistics Statistics { get; private set; } = new Statistics();

        public async Task OnGetAsync()
        {
            try
            {
                // Fetch statistics asynchronously
                var customerCountTask = await _transactionService.GetCustomerCountAsync();
                var accountCountTask = await _transactionService.GetAccountCountAsync();
                var totalBalanceTask = await _transactionService.GetTotalBalanceAsync();

                // Run all tasks concurrently
                
                //await Task.WhenAll(customerCountTask, accountCountTask, totalBalanceTask);

                // Assign results
                Statistics.CustomerCount = customerCountTask;
                Statistics.AccountCount = accountCountTask;
                Statistics.TotalBalance = totalBalanceTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch statistics");
                Statistics = new Statistics
                {
                    CustomerCount = -999,
                    AccountCount = -999,
                    TotalBalance = -999
                };
            }
        }
    }
}
