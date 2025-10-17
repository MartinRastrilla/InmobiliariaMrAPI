namespace InmobiliariaMrAPI.Models.Inmueble;

public class Contrato
{
    public int Id { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal TotalPrice { get; set; }
    public decimal? MonthlyPrice { get; set; }
    public ContratoStatus Status { get; set; } = ContratoStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    //? Navigation Properties
    public int InmuebleId { get; set; }
    public Inmueble Inmueble { get; set; } = null!;

    public int PropietarioId { get; set; }
    public Propietario Propietario { get; set; } = null!;

    public ICollection<ContratoInquilino> ContratoInquilinos { get; set; } = new List<ContratoInquilino>();

    public ICollection<Pagos> Pagos { get; set; } = new List<Pagos>();
}

public enum ContratoStatus
{
    Pending,
    Active,
    Expired,
    Cancelled
}