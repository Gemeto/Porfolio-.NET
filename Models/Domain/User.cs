namespace PorfolioWeb.Models.Domain;

public partial class User
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Surname { get; set; }

    public string? Description { get; set; }

    public int? MainImageId { get; set; }

    public virtual ICollection<JobExperience> JobExperiences { get; set; } = new List<JobExperience>();

    public virtual Image? MainImage { get; set; }
}
