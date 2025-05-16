using Microsoft.EntityFrameworkCore;
using GolfScoreCard.APISTUFF; // where Course, Hole, TeeSet, etc. live
using GolfScoreCard;          // where User & Score live (adjust if in different namespace)

namespace GolfScoreCard
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opts)
            : base(opts)
        { }

        public DbSet<User>   Users   { get; set; }
        public DbSet<Score>  Scores  { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Hole>   Holes   { get; set; }

        // if you need DbSet<TeeSet> or DbSet<Location> etc. add them here
    }
}