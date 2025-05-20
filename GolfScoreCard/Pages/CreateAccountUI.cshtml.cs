using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GolfScoreCard;
using System.Security.Cryptography;
using System.Text;

namespace GolfScoreCard.Pages;

public class CreateAccountUIModel : PageModel
{
    private readonly AppDbContext _db;

    public CreateAccountUIModel(AppDbContext db)
    {
        _db = db;
    }

    [BindProperty] public string Username { get; set; }
    [BindProperty] public string Password { get; set; }
    [BindProperty] public string Sex { get; set; }
    [BindProperty] public decimal Handicap { get; set; }

    public void OnGet() {}

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        // Check if username already exists
        var existingUser = await _db.Users.FindAsync(Username);
        if (existingUser != null)
        {
            ModelState.AddModelError(string.Empty, "Username already taken.");
            return Page();
        }

        // Hash the password
        var hash = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(Password)));

        var newUser = new User
        {
            username = Username,
            passwordHash = hash,
            sex = Sex,
            handicap = Handicap
        };

        _db.Users.Add(newUser);
        await _db.SaveChangesAsync();

        return RedirectToPage("/LoginUI");
    }
}