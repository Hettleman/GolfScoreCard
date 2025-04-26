namespace GolfScoreCard;
public class Tees
{
    public List<FemaleTee> female { get; set; }
}

public class FemaleTee
{
    public string tee_name { get; set; }
    public double course_rating { get; set; }
    public int slope_rating { get; set; }
    public double bogey_rating { get; set; }
    public int total_yards { get; set; }
    public int total_meters { get; set; }
    public int number_of_holes { get; set; }
    public int par_total { get; set; }
}

