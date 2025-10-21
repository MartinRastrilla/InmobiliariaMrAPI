using System.ComponentModel.DataAnnotations;
using InmobiliariaMrAPI.Models.Inmueble;

namespace InmobiliariaMrAPI.Models.User;

public class User
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = null!;

    [Required]
    [EmailAddress]
    [MaxLength(100)]
    public string Email { get; set; } = null!;

    [Required]
    [MaxLength(300)]
    public string Password { get; set; } = null!;
    public bool IsActive { get; set; } = true;

    public string? ProfilePicRoute { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    //? Navigation Properties
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}