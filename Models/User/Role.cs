using System.ComponentModel.DataAnnotations;

namespace InmobiliariaMrAPI.Models.User;

public class Role
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;

    [Required]
    [MaxLength(500)]
    public string Description { get; set; } = null!;

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    //? Navigation Properties
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}