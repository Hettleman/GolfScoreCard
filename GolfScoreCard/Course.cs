namespace GolfScoreCard;

public class Course
{
    public int id { get; set; }
    public string club_name { get; set; }
    public string course_name { get; set; }
    public Location location { get; set; }
    public Tees tees { get; set; }   // << ADD THIS LINE
}