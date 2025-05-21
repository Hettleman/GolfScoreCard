using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GolfScoreCard;

[Table("Scores")]
public class Score
{
    [Key]
    [Column("score_id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int scoreId { get; set; }

    [Required]
    [MaxLength(30)]
    [Column("username")]
    public string username { get; set; }

    [NotMapped]
    [ForeignKey("username")]
    public User? User { get; set; }

    [Required]
    [MaxLength(50)]
    [Column("course_name")]
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