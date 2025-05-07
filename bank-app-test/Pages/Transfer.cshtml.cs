
using DataAccessLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankApp.Pages
{
    public class TransferModel : PageModel
    {
        private readonly TransactionService _transactionService;

        public TransferModel(TransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [BindProperty]
        public int FromAccountId { get; set; }

        [BindProperty]
        public int ToAccountId { get; set; }

        [BindProperty]
        public decimal Amount { get; set; }

        public string Message { get; set; }

        public List<Services.ViewModels.TransactionViewModel> RecentTransactions { get; set; } = new List<Services.ViewModels.TransactionViewModel>();

        /// <summary>
        /// Handles the GET request to fetch recent transactions.
        /// </summary>
        public async Task<IActionResult> OnGetAsync()
        {
            if (FromAccountId > 0) // Ensure a valid account ID is provided
            {
                RecentTransactions = await _transactionService.GetRecentTransactions(FromAccountId);
            }
            return Page();
        }

        /// <summary>
        /// Handles the transfer operation when the form is submitted.
        /// </summary>
        public async Task<IActionResult> OnPostAsync()
        {
            // Validate Amount
            if (Amount <= 0)
            {
                ModelState.AddModelError(string.Empty, "Please provide a valid amount greater than zero.");
            }

            // Validate Accounts
            if (FromAccountId == ToAccountId)
            {
                ModelState.AddModelError(string.Empty, "Transfer accounts must be different.");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Create transfer model
            var model = new Services.ViewModels.TransferViewModel
            {
                FromAccountId = FromAccountId,
                ToAccountId = ToAccountId,
                Amount = Amount
            };

            // Perform transfer operation
            bool transferSuccessful = await _transactionService.PerformTransfer(model);
            if (transferSuccessful)
            {
                Message = "Transfer successful.";
                ModelState.Clear();

                // Reset form fields
                FromAccountId = 0;
                ToAccountId = 0;
                Amount = 0;

                // Refresh recent transactions for the FromAccountId
                RecentTransactions = await _transactionService.GetRecentTransactions(model.FromAccountId);
            }
            else
            {
                Message = "Transfer failed. Please try again.";
            }

            return Page();
        }
    }
}
