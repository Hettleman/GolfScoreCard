using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GolfScoreCard.Pages
{
    public class LogoutUIModel : PageModel
    {
        public void OnGet()
        {
            HttpContext.Session.Clear();
        }
    }
}