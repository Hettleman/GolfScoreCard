using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GolfScoreCard;
using System.Security.Cryptography;
using System.Text;

namespace GolfScoreCard.Pages;

public class LoginUIModel : PageModel
{
    private readonly AppDbContext _db;

    public LoginUIModel(AppDbContext db)
    {
        _db = db;
    }

    [BindProperty] public string Username { get; set; }
    [BindProperty] public string Password { get; set; }

    public void OnGet() { }

    public IActionResult OnPost()
    {
        var hash = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(Password)));

        var user = _db.Users.FirstOrDefault(u => u.username == Username && u.passwordHash == hash);

        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Invalid username or password.");
            return Page();
        }

        // TODO: set session/cookie
        HttpContext.Session.SetString("username", Username);
        return RedirectToPage("/Index");
    }
}