using BankApp.DataAccessLayer.Models;
using BankApp.Services.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankApp.Services.Enums;
using System.ComponentModel;
using BankApp.Services.Utils;

namespace BankApp.Services
{
    public class AccountService : IAccountService
    {
        private readonly BankAppDataContext _context;

        public AccountService(BankAppDataContext context)
        {
            _context = context;
        }

        public async Task<AccountViewModel?> GetAccountByIdAsync(int id)
        {
            var account = await _context.Accounts
                .SingleOrDefaultAsync(a => a.AccountId == id);


            if (account == null)
                return null;

            return new AccountViewModel
            {
                AccountId = account.AccountId,
                Balance = account.Balance,
            };
        }
    }
}