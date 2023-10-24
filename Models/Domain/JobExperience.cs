using PorfolioWeb.Models.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PorfolioWeb.Models.Domain;

public class JobExperience
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    [ForeignKey("Image")]
    public int? ImageId { get; set; }

    public virtual Image? Image { get; set; }

    [Required]
    [ForeignKey("User")]
    public int UserId { get; set; }

    public virtual User? User { get; set; } = null!;
}
