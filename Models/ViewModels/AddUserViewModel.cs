using System.ComponentModel.DataAnnotations;

namespace PorfolioWeb.Models;

public partial class AddUserViewModel
{
    [Required]
    [StringLength(255)]
    public string Email { get; set; } = null!;

    [Required]
    [StringLength(255)]
    public string Password { get; set; } = null!;
    [StringLength(255)]
    public string? Name { get; set; } = null!;
    [StringLength(255)]
    public string? Surname { get; set; }}
