namespace WebApplicationTest.Models.Domain;

public partial class User
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Surname { get; set; }

    public virtual ICollection<JobExperience> JobExperiences { get; set; } = new List<JobExperience>();
}
