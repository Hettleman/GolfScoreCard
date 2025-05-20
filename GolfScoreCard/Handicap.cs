using System;
using System.Collections.Generic;
using System.Linq;

namespace Test
{
    // 1. Strategy interface
    public interface IHandicapStrategy
    {
        double ComputeHandicap(
            List<double> scores,
            List<double> slopes,
            List<double> ratings
        );
    }

    // 2. Excellence strategy (uses 0.96 factor)
    public class ExcellenceHandicapStrategy : IHandicapStrategy
    {
        public double ComputeHandicap(List<double> scores, List<double> slopes, List<double> ratings)
        {
            var topEight = new List<double>();

            for (int i = 0; i < scores.Count; i++)
            {
                var diff = CalculateDifferential(scores[i], slopes[i], ratings[i]);

                if (topEight.Count < 8)
                {
                    topEight.Add(diff);
                    topEight.Sort();
                }
                else if (diff < topEight[7])
                {
                    InsertDifferential(topEight, diff);
                }
            }

            // average the eight lowest differentials, then apply 0.96
            return topEight.Average() * 0.96;
        }

        private static double CalculateDifferential(double score, double slope, double rating) =>
            (score - rating) * (113.0 / slope);

        private static void InsertDifferential(List<double> list, double diff)
        {
            int idx = list.BinarySearch(diff);
            if (idx < 0) idx = ~idx;
            list.Insert(idx, diff);
            list.RemoveAt(8);
        }
    }

    // 3. Basic strategy (no 0.96 factor)
    public class BasicHandicapStrategy : IHandicapStrategy
    {
        public double ComputeHandicap(List<double> scores, List<double> slopes, List<double> ratings)
        {
            // compute all differentials
            var diffs = scores
                .Select((score, i) => CalculateDifferential(score, slopes[i], ratings[i]))
                .ToList();

            // sort ascending, take lowest 8
            diffs.Sort();
            var lowestEight = diffs.Take(8);

            // simple average, no 0.96 factor
            return lowestEight.Average();
        }

        private static double CalculateDifferential(double score, double slope, double rating) =>
            (score - rating) * (113.0 / slope);
    }

    // 5. Application entry point
    public static class Program
    {
        public static void Main()
        {
            var scores = new List<double>
            {
                72, 73, 73, 74, 74, 74, 75, 76, 77, 79,
                80, 81, 81, 81, 82, 83, 83, 84, 85, 86
            };
            var slopes = new List<double>
            {
                113, 123, 119, 110, 134, 123, 112, 132, 133, 123,
                142, 123, 130, 128, 120, 121, 114, 132, 154, 123
            };
            var ratings = new List<double>
            {
                72, 71, 72, 72, 71, 72, 73, 71, 71, 72,
                70, 71, 72, 73, 72, 71, 71, 72, 72, 72
            };

            var excellence = new ExcellenceHandicapStrategy();
            double h1 = excellence.ComputeHandicap(scores, slopes, ratings); ;

            var basic = new BasicHandicapStrategy();
            double h2 = basic.ComputeHandicap(scores, slopes, ratings);

        }
    }
}
