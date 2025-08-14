using BankApp.Services.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BankApp.Services;
using BankApp.Services.ViewModels;
using Microsoft.AspNetCore.Authorization;


namespace BankApp.Pages
{
    [Authorize(Roles = "Cashier")]
    public class DepositModel : PageModel
    {
        private readonly ITransactionService _transactionService;

        public DepositModel(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [BindProperty]
        public int AccountId { get; set; }

        [BindProperty]
        public decimal Amount { get; set; }

        public string Message { get; set; }

        public List<Services.ViewModels.TransactionViewModel> RecentTransactions { get; set; }

        public async Task OnGetAsync()
        {
            RecentTransactions = await _transactionService.GetRecentTransactionsAsync(AccountId);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Amount <= 0)
            {
                ModelState.AddModelError(string.Empty, "Please provide a valid amount.");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }


            bool depositSuccessful = await _transactionService.PerformActionAsync(AccountId, Services.Enums.ActionType.Deposit, Amount);
            if (depositSuccessful)
            {
                Message = "Deposit successful.";
                ModelState.Clear();
                AccountId = 0;
                Amount = 0;


                RecentTransactions = await _transactionService.GetRecentTransactionsAsync(AccountId);
            }
            else
            {
                Message = "Deposit failed. Please try again.";
            }

            return Page();
        }
    }
}












