using System;
using System.Collections.Generic;
using Xunit;
using GolfScoreCard;
using GolfScoreCard.APISTUFF;

namespace API_Testing
{
    public class UnitTest2
    {
        //------------- DIFFERENTIAL --------------

        [Fact]
        public void CalculateDifferential_NonZero()
        {
            // (85 - 72) * 113 / 120 = 13 * 113 / 120 ≈ 12.2583
            double score = 85;
            double slope = 120;
            double rating = 72;
            double expected = (score - rating) * 113.0 / slope;
            
            var actual = GolfScoreCard.Handicap.CalculateDifferential(score, slope, rating);
            Assert.Equal(expected, actual, 4); // precision to 4 decimal places
        }

        //------------- INSERT DIFFERENTIAL ------------

        [Fact]
        public void InsertDifferential_WhenHigherThanAll_DoesNotChangeList()
        {
            var diffs = new List<double> { 2, 3, 4, 5, 6, 7, 8, 9 };
            var original = new List<double>(diffs);
            
            // inserting a large diff should leave the list unchanged
            var result = Handicap.InsertDifferential(diffs, 15);
            Assert.Equal(original, result);
        }

        [Fact]
        public void InsertDifferential_MaintainsSortedOrder()
        {
            var diffs = new List<double> { 1, 2, 4, 5, 6, 7, 8, 9 };
            var expected = new List<double> { 1, 2, 3, 4, 5, 6, 7, 8 };
            
            var result = Handicap.InsertDifferential(diffs, 3);
            Assert.Equal(expected, result);
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
            var strategy = new ExcellenceHandicapStrategy();
            var handicap = strategy.ComputeHandicap(scores, slopes, ratings);

            Assert.Equal(14.25, Math.Round(handicap, 2));
        }

        [Fact]
        public void ExcellenceHandicapStrategy_WithFewerThanEightRounds()
        {
            var scores = new List<double> { 80, 85, 90 };
            var slopes = new List<double>  { 113, 113, 113 };
            var ratings = new List<double>{ 72, 72, 72 };

            // diffs: 8, 13, 18 => average = (8+13+18)/3 = 13
            var strategy = new ExcellenceHandicapStrategy();
            var handicap = strategy.ComputeHandicap(scores, slopes, ratings);

            Assert.Equal(13.0, handicap);
        }
    }
}
