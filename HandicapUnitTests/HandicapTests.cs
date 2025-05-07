namespace HandicapUnitTests;
using GolfScoreCard;

public class HandicapTests
{
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
}