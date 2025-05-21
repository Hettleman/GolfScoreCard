namespace BackendTesting;
using GolfScoreCard.Observers;
using Xunit;
using System;
using System.IO;
using GolfScoreCard;

public class HandicapObserverTests
{
    [Fact]
    public void HandicapChange_ShouldPrintMessage_WhenHandicapDropsBelow10()
    {
        // Arrange
        var user = new User { username = "golfer1" };
        var observer = new HandicapObserver();

        using var sw = new StringWriter();
        Console.SetOut(sw);

        // Act
        observer.HandicapChange(user, 12m, 9.5m);

        // Assert
        var output = sw.ToString().Trim();
        Assert.Equal("Congratulations golfer1! Your handicap is below 10!", output);
    }

    [Fact]
    public void HandicapChange_ShouldNotPrintMessage_WhenHandicapDoesNotDropBelow10()
    {
        // Arrange
        var user = new User { username = "golfer2" };
        var observer = new HandicapObserver();

        using var sw = new StringWriter();
        Console.SetOut(sw);

        // Act
        observer.HandicapChange(user, 9m, 8m); // Already below 10
        var output1 = sw.ToString().Trim();

        sw.GetStringBuilder().Clear();
        observer.HandicapChange(user, 12m, 10m); // Still 10
        var output2 = sw.ToString().Trim();

        // Assert
        Assert.True(string.IsNullOrEmpty(output1));
        Assert.True(string.IsNullOrEmpty(output2));
    }
}