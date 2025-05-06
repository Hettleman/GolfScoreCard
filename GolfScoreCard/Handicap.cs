using System;
using System.Collections.Generic;
using System.Linq;

namespace GolfScoreCard
{
    public class Handicap
    { 
        public static double HandicapCalc(List<double> scores, List<double> slopes, List<double> ratings)
        {
            double handicap;
            //Calculate for par 72 course
            var topEight = new List<double>();
            for (int i = 0; i < scores.Count; i++)
            {
                double thisDifferential = CalculateDifferential(scores[i], slopes[i], ratings[i]);

                if (topEight.Count < 8)
                {
                    topEight.Add(thisDifferential);
                    topEight.Sort();
                }
                else
                {
                    if (thisDifferential < topEight[7])
                    {
                        int insertIdx = topEight.BinarySearch(thisDifferential);
                        if (insertIdx < 0) 
                            insertIdx = ~insertIdx;      // ~ gives the bitwise complement → correct insertion point
                
                        topEight.Insert(insertIdx, thisDifferential);
                        topEight.RemoveAt(8);
                    }
                }
            }

            for (int i = 0; i < 8; i++)
            {
                Console.WriteLine(topEight[i]);
            }

            handicap = CalculateHandicap(topEight);
            Console.WriteLine($"Your handicap based on your last 20 rounds: {handicap}");

            return 0; 
        }

    private static double CalculateDifferential(double score, double slope, double rating)
    {
        double differential;
        differential = ((score - rating) * (113 / slope));
        return differential;
    }

    private static double CalculateHandicap(List<double> diffs)
    {
        double total = 0;
        for (int i = 0; i < diffs.Count; i++)
        {
            total += diffs[i];
        }

        double handicap = ((total / 8) * .96);
        return handicap;
    }

    public static void Main(string[] args)
    {
        // define your 20-round data
        var scores = new List<double>
        { 72, 73, 73, 74, 74, 74, 75, 76, 77, 79,
            80, 81, 81, 81, 82, 83, 83, 84, 85, 86 };
        var slopes = new List<double>
        { 113, 123, 119, 110, 134, 123, 112, 132, 133, 123,
            142, 123, 130, 128, 120, 121, 114, 132, 154, 123 };
        var ratings = new List<double>
        { 72, 71, 72, 72, 71, 72, 73, 71, 71, 72,
            70, 71, 72, 73, 72, 71, 71, 72, 72, 72 };

    
        HandicapCalc(scores, slopes, ratings);
    }
    
    }  

}  