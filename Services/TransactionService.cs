using DataAccessLayer.Models;
using Services.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services
{
    public class TransactionService
    {
        private readonly BankAppDataContext _context;

        public TransactionService(BankAppDataContext context)
        {
            _context = context;
        }

        public async Task<bool> PerformTransaction(TransactionViewModel model)
        {
            var account = await _context.Accounts.SingleOrDefaultAsync(a => a.AccountId == model.AccountId);
            if (account == null) return false;

            string transactionType = model.Type switch
            {
                "Deposit" => "Credit",
                "Withdraw" => "Debit",
                _ => throw new ArgumentException("Invalid transaction type.")
            };

            switch (model.Type)
            {
                case "Deposit":
                    account.Balance += model.Amount;
                    break;
                case "Withdraw":
                    if (account.Balance < model.Amount) return false;
                    account.Balance -= model.Amount;
                    break;
                case "Transfer":
                    // Transfer handled separately
                    break;
            }

            _context.Transactions.Add(new Transaction
            {
                AccountId = account.AccountId,
                Amount = model.Amount,
                Date = DateOnly.FromDateTime(DateTime.Now),
                Type = transactionType,
                Operation = transactionType,
                Balance = account.Balance
            });

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> PerformTransfer(TransferViewModel model)
        {
            var fromAccount = await _context.Accounts.SingleOrDefaultAsync(a => a.AccountId == model.FromAccountId);
            var toAccount = await _context.Accounts.SingleOrDefaultAsync(a => a.AccountId == model.ToAccountId);
            if (fromAccount == null || toAccount == null) return false;
            if (fromAccount.Balance < model.Amount) return false;

            fromAccount.Balance -= model.Amount;
            toAccount.Balance += model.Amount;

            _context.Transactions.Add(new Transaction
            {
                AccountId = fromAccount.AccountId,
                Amount = model.Amount,
                Date = DateOnly.FromDateTime(DateTime.Now),
                Type = "Debit",
                Operation = "Debit",
                Balance = fromAccount.Balance
            });

            _context.Transactions.Add(new Transaction
            {
                AccountId = toAccount.AccountId,
                Amount = model.Amount,
                Date = DateOnly.FromDateTime(DateTime.Now),
                Type = "Credit",
                Operation = "Credit",
                Balance = toAccount.Balance
            });

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<TransactionViewModel>> GetRecentTransactions(int accountId, int count = 10)
        {
            var transactions = await _context.Transactions
                .Where(t => t.AccountId == accountId)
                .OrderByDescending(t => t.TransactionId)
                .Take(count)
                .Select(t => new TransactionViewModel
                {
                    AccountId = t.AccountId,
                    Amount = t.Amount,
                    Type = t.Type,
                    Date = t.Date.ToDateTime(new TimeOnly())
                }).ToListAsync();

            foreach (var transaction in transactions)
            {
                transaction.Type = transaction.Type switch
                {
                    "Credit" => "Deposit",
                    "Debit" => "Withdraw",
                    _ => transaction.Type
                };
            }

            return transactions;
        }

        public async Task<List<Transaction>> GetAllTransactions()
        {
            return await _context.Transactions.ToListAsync();
        }

        public async Task<int> GetCustomerCountAsync()
        {
            return await _context.Customers.CountAsync();
        }

        public async Task<int> GetAccountCountAsync()
        {
            return await _context.Accounts.CountAsync();
        }

        public async Task<decimal> GetTotalBalanceAsync()
        {
            return await _context.Accounts.SumAsync(a => a.Balance);
        }
    }
}