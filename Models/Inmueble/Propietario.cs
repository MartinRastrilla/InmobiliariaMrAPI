using System.ComponentModel.DataAnnotations;

namespace InmobiliariaMrAPI.Models.Inmueble;

public class Propietario
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User.User User { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;
    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = null!;

    [Required]
    [MaxLength(15)]
    public string DocumentNumber { get; set; } = null!;

    [Required]
    [MaxLength(15)]
    public string Phone { get; set; } = null!;

    [Required]
    [EmailAddress]
    [MaxLength(100)]
    public string Email { get; set; } = null!;

    public ICollection<Inmueble> Inmuebles { get; set; } = new List<Inmueble>();
    public ICollection<Contrato> Contratos { get; set; } = new List<Contrato>();
}