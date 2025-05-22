using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GolfScoreCard;


[Table("Users")] //Users class mapped to SQL table
public class User
{
    [Key]
    [MaxLength(30)]
    public string username { get; set; }

    [Column("password_hash")] //hash of password stored, not the actual password
    [Required]
    [MaxLength(255)]
    public string passwordHash { get; set; }

    [Required]
    [MaxLength(6)]
    public string sex { get; set; }
    
    [Required]
    public decimal handicap { get; set; }
}
