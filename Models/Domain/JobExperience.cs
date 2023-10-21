using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplicationTest.Models.Domain;

public class JobExperience
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Title { get; set; } = null!;

    public string? Description { get; set; }
    [Required]
    [ForeignKey("User")]
    public int UserId { get; set; }

    public virtual User? User { get; set; } = null!;
}
