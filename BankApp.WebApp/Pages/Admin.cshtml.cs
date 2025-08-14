using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BankApp.Pages
{
    [Authorize(Roles = "Admin")]
    public class AdminIndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
