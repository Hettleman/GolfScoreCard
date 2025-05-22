using Microsoft.EntityFrameworkCore;
namespace GolfScoreCard
{
    public class AppDbContext : DbContext //Class for establishing connection to database and mapping User and Score classes
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Score> Scores { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) //Constructor for establishing connection to database
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) //Method for configuring the database
        {
            base.OnModelCreating(modelBuilder);

            // User entity configuration
            modelBuilder.Entity<User>()
                .HasKey(u => u.username);

            modelBuilder.Entity<User>()
                .Property(u => u.username)
                .HasMaxLength(30)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(u => u.passwordHash)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(u => u.sex)
                .HasMaxLength(6)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(u => u.handicap)
                .HasColumnType("decimal(4,2)") // this makes sure EF maps it precisely to decimal(4,2)
                .IsRequired();
            
            modelBuilder.Entity<Score>()
                .HasKey(s => s.scoreId);

            modelBuilder.Entity<Score>()
                .Property(s => s.username)
                .HasMaxLength(30)
                .IsRequired();

            modelBuilder.Entity<Score>()
                .Property(s => s.courseName)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<Score>()
                .Property(s => s.userScore)
                .IsRequired();

            modelBuilder.Entity<Score>()
                .Property(s => s.coursePar)
                .IsRequired();

            modelBuilder.Entity<Score>()
                .Property(s => s.dateTime)
                .IsRequired();

            // Foreign Key relationship
            modelBuilder.Entity<Score>()
                .HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.username)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete if user is deleted
        }
    }
}