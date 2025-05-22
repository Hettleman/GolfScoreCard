namespace GolfScoreCard.Observers;

public interface IUserObserver
{
    public interface IUserObserver //Interface for observer that acts upon handicap change
    {
        void HandicapChange(User user, decimal oldHandicap, decimal newHandicap);
    }
}