using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BankApp.DataAccessLayer.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using BankApp.Services;
using BankApp.Services.ViewModels;
using Microsoft.AspNetCore.Authorization;



namespace BankApp.Pages
{
    [Authorize(Roles = "Cashier")]
    public class AccountDetailsModel : PageModel
    {
        private readonly ILogger<AccountDetailsModel> _logger;
        private readonly ITransactionService _transactionService;
        private readonly IAccountService _accountService;

        public AccountDetailsModel(IAccountService accountService, ITransactionService transactionService, ILogger<AccountDetailsModel> logger)
        {
            _accountService = accountService;
            _transactionService = transactionService;
            _logger = logger;
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public AccountViewModel? Account { get; set; }
        public List<TransactionViewModel> Transactions { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            Account = await _accountService.GetAccountByIdAsync(Id);

            if (Account == null)
                return NotFound();

            Transactions = await _transactionService.GetAccountTransactionsSliceAsync(Account.AccountId, skip: 0, take: 20);

            return Page();
        }

        /// <summary>
        /// Loads more transactions when requested via AJAX.
        /// </summary>
        public async Task<IActionResult> OnGetLoadMoreAsync(int id, int skip, int take = 20)
        {
            var moreTransactions = await _transactionService.GetAccountTransactionsSliceAsync(id, skip, take);
            return Partial("_TransactionRows", moreTransactions);
        }
    }
}