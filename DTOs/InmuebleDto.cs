namespace InmobiliariaMrAPI.DTOs;

public class InmuebleDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string? Latitude { get; set; } = null!;
    public string? Longitude { get; set; } = null!;
    public int Rooms { get; set; }
    public decimal Price { get; set; }
    public int? MaxGuests { get; set; }
    public bool Available { get; set; } = true;
    public List<IFormFile>? Images { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public List<string>? ArchivosRoutes { get; set; }
}