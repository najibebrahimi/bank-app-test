

using BankApp.Services.ViewModels;

namespace BankApp.Services
{
    public interface IStatisticsService
    {
        Task<StatisticsViewModel> GetStatisticsAsync();
    }
}