using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GolfScoreCard;

[Table("Scores")] //scores class mapped to SQL table
public class Score
{
    [Key]
    [Column("score_id")] //Primary key for scores table
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int scoreId { get; set; }

    [Required] //This is passed in via the corresponding user
    [MaxLength(30)]
    [Column("username")]
    public string username { get; set; }

    [NotMapped] //Foreign key relationship with User via username
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