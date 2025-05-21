using Xunit;
using Microsoft.EntityFrameworkCore;
using GolfScoreCard.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Collections.Generic;
namespace BackendTesting;
using GolfScoreCard;

public class ControllerTests
{
    private AppDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }
    

    [Fact]
    public async Task CreateUser_ShouldReturnOk_WhenValid()
    {
        var context = GetInMemoryDbContext();
        var controller = new UsersController(context);

        var user = new User
        {
            username = "testuser",
            passwordHash = "plaintextpassword",
            sex = "male",  
            handicap = 0     
        };

        var result = await controller.CreateUser(user);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task CreateUser_ShouldReturnBadRequest_WhenUsernameTooShort()
    {
        var context = GetInMemoryDbContext();
        var controller = new UsersController(context);

        var user = new User
        {
            username = "ab",
            passwordHash = "password123",
            sex = "male",
            handicap = 0   
        };

        var result = await controller.CreateUser(user);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Login_ShouldReturnUnauthorized_WhenWrongPassword()
    {
        var context = GetInMemoryDbContext();
        var controller = new UsersController(context);

        var user = new User
        {
            username = "loginuser",
            passwordHash = BCrypt.Net.BCrypt.HashPassword("correctpass"),
            sex = "male",    
            handicap = 0     
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var login = new LoginRequest
        {
            Username = "loginuser",
            Password = "wrongpass"
        };

        var result = await controller.Login(login);
        Assert.IsType<UnauthorizedObjectResult>(result);
    }

    [Fact]
    public async Task DeleteUser_ShouldReturnNotFound_IfUserDoesNotExist()
    {
        var context = GetInMemoryDbContext();
        var controller = new UsersController(context);

        var result = await controller.DeleteUser("ghost");
        Assert.IsType<NotFoundObjectResult>(result);
    }
    

    [Fact]
    public async Task AddScore_ShouldReturnBadRequest_WhenUserDoesNotExist()
    {
        var context = GetInMemoryDbContext();
        var controller = new ScoresController(context);

        var score = new Score
        {
            username = "nouser",
            courseName = "Pebble Beach",
            userScore = 72,
            coursePar = 72
        };

        var result = await controller.AddScore(score);
        Assert.IsType<BadRequestObjectResult>(result);
    }
    
    [Fact]
    public async Task AddScore_ShouldReturnOk_WhenValid()
    {
        var context = GetInMemoryDbContext();
        context.Users.Add(new User { 
            username = "player1", 
            passwordHash = "hashed",
            sex = "male",  
            handicap = 0     
        });
        await context.SaveChangesAsync();

        var controller = new ScoresController(context);

        var score = new Score
        {
            username = "player1",
            courseName = "Augusta",
            userScore = 70,
            coursePar = 72
        };

        var result = await controller.AddScore(score);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task DeleteScore_ShouldReturnNotFound_IfScoreNotExists()
    {
        var context = GetInMemoryDbContext();
        var controller = new ScoresController(context);

        var result = await controller.DeleteScore(999);
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task GetScores_ShouldReturnUserScores()
    {
        var context = GetInMemoryDbContext();
        context.Users.Add(new User { 
            username = "playerX", 
            passwordHash = "abc",
            sex = "male", 
            handicap = 0  
        });
        context.Scores.Add(new Score { username = "playerX", courseName = "Torrey Pines", userScore = 74, coursePar = 72 });
        await context.SaveChangesAsync();

        var controller = new ScoresController(context);
        var result = await controller.GetScores("playerX") as OkObjectResult;

        Assert.NotNull(result);
        var scores = Assert.IsType<List<Score>>(result.Value);
        Assert.Single(scores);
    }
    
    [Fact]
    public async Task UpdateHandicap_ShouldReturnNotFound_IfUserDoesNotExist()
    {
        var context = GetInMemoryDbContext();
        var controller = new UsersController(context);

        var result = await controller.UpdateHandicap("missinguser", 12.5m);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task UpdateHandicap_ShouldUpdateAndReturnOk_WhenUserExists()
    {
        var context = GetInMemoryDbContext();

        var user = new User
        {
            username = "golfer1",
            passwordHash = "securehash",
            sex = "male",
            handicap = 10m
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        var controller = new UsersController(context);
        var result = await controller.UpdateHandicap("golfer1", 15m);
        
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode ?? 200); // optional line
        
        var updatedUser = await context.Users.FindAsync("golfer1");
        Assert.NotNull(updatedUser);
        Assert.Equal(15m, updatedUser.handicap);
    }

}