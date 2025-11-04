namespace InmobiliariaMrAPI.DTOs;

public class InquilinoDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string DocumentNumber { get; set; } = null!;
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

