namespace GolfScoreCard.Logic
{
    public sealed class HandicapCalculator
    {
        private static readonly HandicapCalculator _instance = new HandicapCalculator();
        public static HandicapCalculator Instance => _instance;

        private HandicapCalculator() { }

        public double CalculateDifferential(int score, double courseRating, int courseSlope)
        {
            return (score - courseRating) * 113 / courseSlope;
        }

        public decimal CalculateAverage(IEnumerable<double> differentials)
        {
            return Math.Round((decimal)differentials.Average(), 1);
        }
    }
}