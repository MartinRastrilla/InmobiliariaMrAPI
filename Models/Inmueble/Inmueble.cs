using System.ComponentModel.DataAnnotations;

namespace InmobiliariaMrAPI.Models.Inmueble;

public class Inmueble
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = null!;

    [Required]
    [MaxLength(200)]
    public string Address { get; set; } = null!;

    [Required]
    public int Rooms { get; set; }

    [Required]
    public decimal Price { get; set; }
    
    public int? MaxGuests { get; set; }
    public bool Available { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    //? Navigation Properties
    public int PropietarioId { get; set; }
    public Propietario Propietario { get; set; } = null!;
    public ICollection<Contrato> Contratos { get; set; } = new List<Contrato>();
}