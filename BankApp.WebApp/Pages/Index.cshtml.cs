
using BankApp.Services.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using BankApp.Services;
using BankApp.Services.ViewModels;
using System.Threading.Tasks;

namespace BankApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IStatisticsService _statisticsService;
        private readonly ILogger<IndexModel> _logger;
        public StatisticsViewModel StatisticsViewModel { get; private set; }
        public decimal TotalBalance => StatisticsViewModel.TotalBalance;
        public int AccountCount => StatisticsViewModel.AccountCount;
        public int CustomerCount => StatisticsViewModel.CustomerCount;


        public IndexModel(IStatisticsService statisticsService, ILogger<IndexModel> logger)
        {
            _statisticsService = statisticsService;
            _logger = logger;
        }
        public async Task OnGetAsync()
        {
            StatisticsViewModel = await  _statisticsService.GetStatisticsAsync();
        }
        /*public async Task OnGetAsync()
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
        }*/
    }
}
