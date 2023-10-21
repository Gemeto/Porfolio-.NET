using System.ComponentModel.DataAnnotations;

namespace WebApplicationTest.Models;

public partial class LoginUserViewModel
{
    [Required]
    [StringLength(255)]
    public string Email { get; set; } = null!;

    [Required]
    [StringLength(255)]
    public string Password { get; set; } = null!;
}
