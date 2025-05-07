using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;



namespace BankApp.Pages
{
    public class AccountDetailsModel : PageModel
    {
        private readonly BankAppDataContext _context;
        private readonly ILogger<AccountDetailsModel> _logger;

        public AccountDetailsModel(BankAppDataContext context, ILogger<AccountDetailsModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public Account? Account { get; set; }
        public IList<Transaction> Transactions { get; set; } = new List<Transaction>();

        /// <summary>
        /// Loads account details and the first 20 transactions.
        /// </summary>
        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Invalid account ID: {AccountId}", id);
                return BadRequest("Invalid account ID.");
            }

            try
            {
                Account = await _context.Accounts
                    .AsNoTracking()
                    .FirstOrDefaultAsync(a => a.AccountId == id);

                if (Account == null)
                {
                    _logger.LogWarning("Account not found with ID: {AccountId}", id);
                    return NotFound("Account not found.");
                }

                Transactions = await _context.Transactions
                    .AsNoTracking()
                    .Where(t => t.AccountId == id)
                    .OrderByDescending(t => t.Date)
                    .Take(20)
                    .ToListAsync();
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error loading account details for ID {AccountId}", id);
                return StatusCode(500, "An error occurred while loading account details.");
            }

            return Page();
        }

        /// <summary>
        /// Loads more transactions when requested via AJAX.
        /// </summary>
        public async Task<IActionResult> OnGetLoadMoreAsync(int id, int skip)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Invalid account ID: {AccountId}", id);
                return BadRequest("Invalid account ID.");
            }

            try
            {
                var transactions = await _context.Transactions
                    .AsNoTracking()
                    .Where(t => t.AccountId == id)
                    .OrderByDescending(t => t.Date)
                    .Skip(skip)
                    .Take(20)
                    .ToListAsync();

                if (!transactions.Any())
                {
                    return NotFound("No more transactions available.");
                }

                return new JsonResult(transactions);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error loading more transactions for Account ID {AccountId}", id);
                return StatusCode(500, "An error occurred while loading transactions.");
            }
        }
    }
}