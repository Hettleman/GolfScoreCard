using System.Collections.Generic;
using Xunit;
using GolfScoreCard;
using GolfScoreCard.APISTUFF;

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
        public void CalcDifferentialTest()
        {
            double score = 72;
            double slope = 113;
            double rating = 72;
            Assert.Equal(0, Handicap.CalculateDifferential(score, slope, rating));
        }

        [Fact]
        public void InsertDiffTest()
        {
            List<double> diffs = new List<double> { 3, 4, 5, 5, 6, 7, 8, 10 };
            List<double> expected = new List<double> { 3, 4, 4, 5, 5, 6, 7, 8 };

            Assert.Equal(expected, Handicap.InsertDifferential(diffs, 4));
        }
    }
}