namespace GolfScoreCard;

using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Score> Scores { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlServer("Server=tcp:parpalserver.database.windows.net,1433;Initial Catalog=parpaldb;Persist Security Info=False;User ID=CloudSA63016971;Password=rexzis-piPwip-9xidvu;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
}
