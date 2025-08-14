using BankApp.Services.ViewModels;

namespace BankApp.Services
{
    public interface IAccountService
    {
        Task<AccountViewModel?> GetAccountByIdAsync(int id);
    }
}