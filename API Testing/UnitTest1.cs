using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace API_Testing;
using GolfScoreCard;



public class UnitTest1
{
    /*
    [Fact]
    public void GetParTest()
    {
        Course course = new Course();
        course.course_name = "Pebble Beach";
        
        Assert.Equal(72, FetchFromAPI.GetFemalePar(course));
    }
    */
    

    [Fact] 
    public void GetLocationTest()
   {
        Course course = new Course();
        course.course_name = "Pebble Beach";
        Assert.Equal("CA", FetchFromAPI.GetLocation(course));
    }
    
    //------------- HANDICAP----------
    
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
        
        List<double> diffs = new List<double>();
        diffs.Add(3);
        diffs.Add(4);
        diffs.Add(5);
        diffs.Add(5);
        diffs.Add(6);
        diffs.Add(7);
        diffs.Add(8);
        diffs.Add(10);
        
        List<double> changedDiffs = new List<double>();
        changedDiffs.Add(3);
        changedDiffs.Add(4);
        changedDiffs.Add(4);
        changedDiffs.Add(5);
        changedDiffs.Add(5);
        changedDiffs.Add(6);
        changedDiffs.Add(7);
        changedDiffs.Add(8);
        
        
        Assert.Equal(changedDiffs, Handicap.InsertDifferential(diffs, 4));
    }
    
    
}