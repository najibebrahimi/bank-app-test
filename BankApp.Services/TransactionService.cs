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
    public class TransactionService : ITransactionService
    {
        private readonly BankAppDataContext _context;

        public TransactionService(BankAppDataContext context)
        {
            _context = context;
        }

        public async Task<bool> PerformActionAsync(int accountId, ActionType type, decimal amount)
        {
            var account = await _context.Accounts.SingleOrDefaultAsync(a => a.AccountId == accountId);
            if (account == null) return false;

            string? transactionTypeString = null;

            switch (type)
            {
                case ActionType.Deposit:
                    account.Balance += amount;
                    transactionTypeString = Common.TransactionTypeToString(TransactionType.Credit);
                    break;
                case ActionType.Withdraw:
                    if (account.Balance < amount) return false;
                    account.Balance -= amount;
                    transactionTypeString = Common.TransactionTypeToString(TransactionType.Debit);
                    break;
                case ActionType.Transfer:
                    throw new InvalidEnumArgumentException($"Transfers are handled in `PerformTransfer`");
                default:
                    throw new InvalidEnumArgumentException($"{nameof(type)} is unsupported");
            }

            string actionType = Common.ActionTypeToString(type);

            _context.Transactions.Add(new Transaction
            {
                AccountId = accountId,
                Amount = amount,
                Date = DateOnly.FromDateTime(DateTime.Now),
                Type = transactionTypeString,
                Operation = $"{transactionTypeString} in Cash",
                Balance = account.Balance
            });

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Transaction failed: {ex}");
                return false;
            }
        }

        public async Task<bool> PerformTransferAsync(TransferViewModel model)
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
                Type = Common.TransactionTypeToString(TransactionType.Debit),
                Operation = $"{Common.TransactionTypeToString(TransactionType.Debit)} in Cash",
                Balance = fromAccount.Balance
            });

            _context.Transactions.Add(new Transaction
            {
                AccountId = toAccount.AccountId,
                Amount = model.Amount,
                Date = DateOnly.FromDateTime(DateTime.Now),
                Type = Common.TransactionTypeToString(TransactionType.Credit),
                Operation = $"{Common.TransactionTypeToString(TransactionType.Credit)} in Cash",
                Balance = toAccount.Balance
            });

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<TransactionViewModel>> GetAccountTransactionsSliceAsync(int accountId, int skip = 0, int take = 20)
        {
            var transactions = await _context.Transactions
                .AsNoTracking()
                .Where(t => t.AccountId == accountId)
                .OrderByDescending(t => t.Date)
                .Skip(skip)
                .Take(take)
                .Select(t => new TransactionViewModel
                {
                    AccountId = accountId,
                    Amount = t.Amount,
                    Type = Common.TransactionTypeStringToEnum(t.Type),
                    Date = t.Date.ToDateTime(new TimeOnly())
                }).ToListAsync();

            return transactions;
        }
    
        public async Task<List<TransactionViewModel>> GetRecentTransactionsAsync(int accountId, int count = 10)
        {
            var transactions = await _context.Transactions
                .Where(t => t.AccountId == accountId)
                .OrderByDescending(t => t.TransactionId)
                .Take(count)
                .Select(t => new TransactionViewModel
                {
                    AccountId = t.AccountId,
                    Amount = t.Amount,
                    Type = Common.TransactionTypeStringToEnum(t.Type),
                    Date = t.Date.ToDateTime(new TimeOnly())
                }).ToListAsync();

            return transactions;
        }

        public async Task<List<Transaction>> GetAllTransactionsAsync()
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