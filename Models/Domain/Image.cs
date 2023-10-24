namespace PorfolioWeb.Models.Domain;

public partial class Image
{
    public int Id { get; set; }

    public string Path { get; set; } = null!;

    public virtual ICollection<JobExperience> JobExperiences { get; set; } = new List<JobExperience>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
