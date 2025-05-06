using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GolfScoreCard;


[Table("Scores")]
public class Score
{
    [Column("score_id")]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int scoreId { get; set; } 

    [ForeignKey("Username")]
    public User User { get; set; }
    
    [Required]
    [MaxLength(30)]
    public string username { get; set; }
    
    [Column("course_name")]
    [Required]
    [MaxLength(50)]
    public string courseName { get; set; }

    [Required]
    [Column("user_score")]
    public int userScore { get; set; }
    
    [Required]
    [Column("course_par")]
    public int coursePar { get; set; }

    [Required]
    [Column("date_time")]
    public DateTime dateTime { get; set; }
}
