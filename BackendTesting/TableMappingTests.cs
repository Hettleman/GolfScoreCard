using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Xunit;
namespace BackendTesting;
using GolfScoreCard;

public class AppDbContextTests
{
    private DbContextOptions<AppDbContext> GetInMemoryOptions()
    {
        return new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public void CanInsertUser()
    {
        var options = GetInMemoryOptions();

        using (var context = new AppDbContext(options))
        {
            var user = new User
            {
                username = "jackson",
                passwordHash = "hashed_password",
                sex = "Male",
                handicap = 5.5m
            };

            context.Users.Add(user);
            context.SaveChanges();
        }

        using (var context = new AppDbContext(options))
        {
            var user = context.Users.SingleOrDefault(u => u.username == "jackson");
            Assert.NotNull(user);
            Assert.Equal("hashed_password", user.passwordHash);
            Assert.Equal(5.5m, user.handicap);
        }
    }

    [Fact]
    public void CanInsertScoreWithUser()
    {
        var options = GetInMemoryOptions();

        using (var context = new AppDbContext(options))
        {
            var user = new User
            {
                username = "jackson",
                passwordHash = "hashed_password",
                sex = "Male",
                handicap = 5.5m
            };

            context.Users.Add(user);
            context.SaveChanges();

            var score = new Score
            {
                username = user.username,
                courseName = "Pebble Beach",
                userScore = 72,
                coursePar = 70,
                dateTime = DateTime.UtcNow
            };

            context.Scores.Add(score);
            context.SaveChanges();
        }

        using (var context = new AppDbContext(options))
        {
            var score = context.Scores.Include(s => s.User).SingleOrDefault();
            Assert.NotNull(score);
            Assert.Equal("jackson", score.username);
            Assert.Equal("Pebble Beach", score.courseName);
            Assert.NotNull(score.User);
            Assert.Equal("jackson", score.User.username);
        }
    }

    [Fact]
    public void DeletingUserDeletesScores()
    {
        var options = GetInMemoryOptions();

        using (var context = new AppDbContext(options))
        {
            var user = new User
            {
                username = "jackson",
                passwordHash = "hashed_password",
                sex = "Male",
                handicap = 5.5m
            };

            context.Users.Add(user);
            context.SaveChanges();

            var score = new Score
            {
                username = user.username,
                courseName = "Pebble Beach",
                userScore = 72,
                coursePar = 70,
                dateTime = DateTime.UtcNow
            };

            context.Scores.Add(score);
            context.SaveChanges();

                // Delete user
            context.Users.Remove(user);
            context.SaveChanges();
        }

        using (var context = new AppDbContext(options))
        {
            // Score should be gone
            var scores = context.Scores.ToList();
            Assert.Empty(scores);
        }
    }
}

