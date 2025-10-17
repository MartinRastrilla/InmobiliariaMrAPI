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
    [EmailAddress]
    [MaxLength(100)]
    public string Email { get; set; } = null!;

    [Required]
    [MaxLength(300)]
    public string Password { get; set; } = null!;
    public bool IsActive { get; set; } = true;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    //? Navigation Properties
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public int? PropietarioId { get; set; }
    public Propietario? Propietario { get; set; }
}