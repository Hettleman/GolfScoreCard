using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace API_Testing;
using GolfScoreCard;

public class UnitTest1
{
    [Fact]
    public void GetParTest()
    {
        Course course = new Course();
        course.course_name = "Pebble Beach";
        
        Assert.Equal(72, FetchFromAPI.GetFemalePar(course));
    }

    [Fact]
    public void GetLocationTest()
    {
        Course course = new Course();
        course.course_name = "Pebble Beach";
        
        Assert.Equal("CA", FetchFromAPI.GetLocation(course));
    }
}