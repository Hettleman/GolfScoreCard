using System;
using System.Collections.Generic;

namespace GolfScoreCard.Strategy
{
    // 1. Strategy interface
    public interface IHandicapStrategy
    {
        double ComputeHandicap(List<double> scores, List<double> slopes, List<double> ratings);
    }

    // Concrete strategy: with .96 excellency
    public class UsgaHandicapStrategy : IHandicapStrategy
    {
        public double ComputeHandicap(List<double> scores, List<double> slopes, List<double> ratings)
        {
            ValidateInputs(scores, slopes, ratings);

            var diffs = CalculateDifferentials(scores, slopes, ratings);
            var topEight = GetLowestDifferentials(diffs, 8);
            return CalculateHandicap(topEight, applyMultiplier: true);
        }

        private void ValidateInputs(List<double> scores, List<double> slopes, List<double> ratings)
        {
            if (scores == null || slopes == null || ratings == null)
                throw new ArgumentNullException("Input lists cannot be null.");
            if (scores.Count != slopes.Count || scores.Count != ratings.Count)
                throw new ArgumentException("Scores, slopes, and ratings must have the same count.");
        }

        private List<double> CalculateDifferentials(List<double> scores, List<double> slopes, List<double> ratings)
        {
            var differentials = new List<double>();
            for (int i = 0; i < scores.Count; i++)
            {
                double diff = (scores[i] - ratings[i]) * (113.0 / slopes[i]);
                differentials.Add(diff);
            }
            return differentials;
        }

        private List<double> GetLowestDifferentials(List<double> diffs, int count)
        {
            diffs.Sort();
            return diffs.GetRange(0, Math.Min(count, diffs.Count));
        }

        private double CalculateHandicap(List<double> diffs, bool applyMultiplier)
        {
            double sum = 0;
            foreach (var d in diffs) sum += d;
            double average = sum / diffs.Count;
            return applyMultiplier ? average * 0.96 : average;
        }
    }

    // without excellency pattern
    public class RawHandicapStrategy : IHandicapStrategy
    {
        public double ComputeHandicap(List<double> scores, List<double> slopes, List<double> ratings)
        {
            if (scores == null || slopes == null || ratings == null)
                throw new ArgumentNullException("Input lists cannot be null.");

            var diffs = new List<double>();
            for (int i = 0; i < scores.Count; i++)
            {
                double diff = (scores[i] - ratings[i]) * (113.0 / slopes[i]);
                diffs.Add(diff);
            }
            diffs.Sort();
            var topEight = diffs.GetRange(0, Math.Min(8, diffs.Count));
            return topEight.Sum() / topEight.Count;
        }
    }

    // 3. Use strategy
    public class HandicapContext
    {
        private readonly IHandicapStrategy _strategy;

        public HandicapContext(IHandicapStrategy strategy)
        {
            _strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
        }

        public double Calculate(List<double> scores, List<double> slopes, List<double> ratings)
        {
            return _strategy.ComputeHandicap(scores, slopes, ratings);
        }
    }

    // 4. Example usage
    class Program
    {
        static void Main(string[] args)
        {
            var scores = new List<double> { 72, 73, 73, 74, 74, 74, 75, 76, 77, 79,
                                            80, 81, 81, 81, 82, 83, 83, 84, 85, 86 };
            var slopes = new List<double> { 113, 123, 119, 110, 134, 123, 112, 132, 133, 123,
                                            142, 123, 130, 128, 120, 121, 114, 132, 154, 123 };
            var ratings = new List<double> { 72, 71, 72, 72, 71, 72, 73, 71, 71, 72,
                                             70, 71, 72, 73, 72, 71, 71, 72, 72, 72 };

            // Strategy with USGA adjustment
            Console.WriteLine("-- USGA Handicap (0.96 multiplier) --");
            var usgaContext = new HandicapContext(new UsgaHandicapStrategy());
            usgaContext.Calculate(scores, slopes, ratings);

            // Strategy without adjustment
            Console.WriteLine("-- Raw Handicap (no multiplier) --");
            var rawContext = new HandicapContext(new RawHandicapStrategy());
            rawContext.Calculate(scores, slopes, ratings);
        }
    }
}
