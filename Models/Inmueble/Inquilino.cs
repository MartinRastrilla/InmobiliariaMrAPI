using System.ComponentModel.DataAnnotations;

namespace InmobiliariaMrAPI.Models.Inmueble;

public class Inquilino
{
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = null!;

    [MaxLength(15)]
    public string DocumentNumber { get; set; } = null!;

    [MaxLength(15)]
    public string? Phone { get; set; } = string.Empty;

    [EmailAddress]
    [MaxLength(100)]
    public string? Email { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    //? Navigation Properties
    public ICollection<Contrato> Contratos { get; set; } = new List<Contrato>();
    public ICollection<ContratoInquilino> ContratoInquilinos { get; set; } = new List<ContratoInquilino>();
}
