using DataAccessLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;
using Services.ViewModels;


namespace BankApp.Pages
{
    public class DepositModel : PageModel
    {
        private readonly TransactionService _transactionService;

        public DepositModel(TransactionService transactionService)
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
            RecentTransactions = await _transactionService.GetRecentTransactions(AccountId);
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

            var model = new Services.ViewModels.TransactionViewModel
            {
                AccountId = AccountId,
                Amount = Amount,
                Type = "Deposit"
            };

            bool depositSuccessful = await _transactionService.PerformTransaction(model);
            if (depositSuccessful)
            {
                Message = "Deposit successful.";
                ModelState.Clear();
                AccountId = 0;
                Amount = 0;


                RecentTransactions = await _transactionService.GetRecentTransactions(model.AccountId);
            }
            else
            {
                Message = "Deposit failed. Please try again.";
            }

            return Page();
        }
    }
}












