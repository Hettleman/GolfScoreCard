namespace GolfScoreCard.Observers;

public class HandicapObserver : IUserObserver //class for observing User handicap changes
{
    public void HandicapChange(User user, decimal oldHandicap, decimal newHandicap)
    {
        if (oldHandicap >= 10 && newHandicap < 10) //check when user handicap is updated and notify (potentially)
        {
            Console.WriteLine($"Congratulations {user.username}! Your handicap is below 10!");
        }
    }
}