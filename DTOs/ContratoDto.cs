namespace InmobiliariaMrAPI.DTOs;

public class ContratoDto
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal TotalPrice { get; set; }
    public decimal? MonthlyPrice { get; set; }
    public string Status { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    //? Información del Inmueble
    public InmuebleInfoDto Inmueble { get; set; } = null!;
    
    //? Información de los Inquilinos
    public List<InquilinoInfoDto> Inquilinos { get; set; } = new List<InquilinoInfoDto>();
}

public class InmuebleInfoDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string? Latitude { get; set; }
    public string? Longitude { get; set; }
    public int Rooms { get; set; }
    public decimal Price { get; set; }
    public int? MaxGuests { get; set; }
    public bool Available { get; set; }
}

public class InquilinoInfoDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string DocumentNumber { get; set; } = null!;
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public bool IsPaymentResponsible { get; set; }
}

