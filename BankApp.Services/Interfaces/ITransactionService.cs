using BankApp.DataAccessLayer.Models;
using BankApp.Services.ViewModels;
using BankApp.Services.Enums;

namespace BankApp.Services
{
    public interface ITransactionService
    {
        Task<bool> PerformActionAsync(int accountId, ActionType type, decimal amount);
        Task<bool> PerformTransferAsync(TransferViewModel model);
        Task<List<TransactionViewModel>> GetAccountTransactionsSliceAsync(int accountId, int skip = 0, int take = 20);
        Task<List<TransactionViewModel>> GetRecentTransactionsAsync(int accountId, int count = 10);
        Task<List<Transaction>> GetAllTransactionsAsync();
        Task<int> GetCustomerCountAsync();
        Task<int> GetAccountCountAsync();
        Task<decimal> GetTotalBalanceAsync();
    }
}