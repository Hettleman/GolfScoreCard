namespace GolfScoreCard.Pages;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class IndexModel : PageModel
{
    [BindProperty]
    public string Name { get; set; }

    public string Message { get; set; }

    public void OnGet()
    {
        Message = "";
    }

    public void OnPost()
    {
        Message = $"Welcome, {Name}!";
    }
}
