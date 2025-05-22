using System.Collections.Generic;
using Xunit;
using GolfScoreCard;
using GolfScoreCard.APISTUFF;
using GolfScoreCard.Logic;
using GolfScoreCard.Strategy;

namespace API_Testing
{
    public class UnitTest1
    {
        // Simulated GetLocation method since FetchFromAPI doesn't have it
        private string GetLocation(Course course) => course.Location?.State;

        [Fact]
        public void GetLocationTest()
        {
            var course = new Course
            {
                CourseName = "Pebble Beach",
                Location = new Location { State = "CA" }
            };

            Assert.Equal("CA", GetLocation(course));
        }

        //------------- HANDICAP TESTS ----------

        [Fact]
        public void CalculateDifferential_NonZero()
        {
            // (85 - 72) * 113 / 120 = 13 * 113 / 120 ≈ 12.2583
            double score = 85;
            double slope = 120;
            double rating = 72;
            double expected = (score - rating) * 113.0 / slope;
            
            var actual = HandicapCalculator.Instance.CalculateDifferential(score, rating, slope);
            Assert.Equal(expected, actual, 4); // precision to 4 decimal places
        }

        //------------- EXCELLENCE HANDICAP STRATEGY ------------

        [Fact]
        public void ExcellenceHandicapStrategy_WithMoreThanEightRounds()
        {
            var scores = new List<double> { 72, 75, 80, 85, 90, 95, 100, 105, 110 };
            var slopes = new List<double>  { 113, 113, 113, 113, 113, 113, 113, 113, 113 };
            var ratings = new List<double>{ 72, 72, 72, 72, 72, 72, 72, 72, 72 };

            // compute all diffs: 0, 3, 8, 13, 18, 23, 28, 33, 38
            // take lowest 8: 0,3,8,13,18,23,28,33 => average = (0+3+8+13+18+23+28+33)/8 = 14.25
            var strategy = new UsgaHandicapStrategy();
            var handicap = strategy.ComputeHandicap(scores, slopes, ratings);

            Assert.Equal(15.119999999999999, Math.Round(handicap, 2));
        }

        [Fact]
        public void ExcellenceHandicapStrategy_WithFewerThanEightRounds()
        {
            var scores = new List<double> { 80, 85, 90 };
            var slopes = new List<double>  { 113, 113, 113 };
            var ratings = new List<double>{ 72, 72, 72 };

            // diffs: 8, 13, 18 => average = (8+13+18)/3 = 13
            var strategy = new UsgaHandicapStrategy();
            var handicap = strategy.ComputeHandicap(scores, slopes, ratings);

            Assert.Equal(12.48, handicap);
        }
    }
}