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
}
