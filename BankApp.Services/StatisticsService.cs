

using BankApp.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using BankApp.Services.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace BankApp.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly BankAppDataContext _context;

        public StatisticsService(BankAppDataContext context)
        {
            _context = context;
        }

        public async Task<StatisticsViewModel> GetStatisticsAsync()
        {
            var customerCount = await _context.Customers.CountAsync();
            var accountCount = await _context.Accounts.CountAsync();
            var totalBalance = await _context.Accounts.SumAsync(a => a.Balance);

            return new StatisticsViewModel
            {
                CustomerCount = customerCount,
                AccountCount = accountCount,
                TotalBalance = totalBalance
            };
        }
    }
}