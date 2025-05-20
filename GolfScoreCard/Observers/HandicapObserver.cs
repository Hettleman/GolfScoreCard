namespace GolfScoreCard.Observers;

public class HandicapObserver : IUserObserver
{
    public void HandicapChange(User user, decimal oldHandicap, decimal newHandicap)
    {
        if (oldHandicap >= 10 && newHandicap < 10)
        {
            Console.WriteLine($"Congratulations {user.username}! Your handicap is below 10!");
        }
    }
}