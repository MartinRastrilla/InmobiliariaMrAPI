namespace InmobiliariaMrAPI.DTOs;

public class PagoDto
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string PaymentMethod { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    //? Información del Contrato
    public ContratoPagoInfoDto Contrato { get; set; } = null!;
    
    //? Información del Inquilino que realizó el pago
    public InquilinoPagoInfoDto Inquilino { get; set; } = null!;
}

public class ContratoPagoInfoDto
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal TotalPrice { get; set; }
    public decimal? MonthlyPrice { get; set; }
    public string Status { get; set; } = null!;
    
    //? Información básica del Inmueble
    public InmuebleInfoDto Inmueble { get; set; } = null!;
}

public class InquilinoPagoInfoDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string DocumentNumber { get; set; } = null!;
    public string? Phone { get; set; }
    public string? Email { get; set; }
}

