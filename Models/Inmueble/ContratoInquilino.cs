namespace InmobiliariaMrAPI.Models.Inmueble;

public class ContratoInquilino
{
    public int Id { get; set; }
    public int ContratoId { get; set; }
    public Contrato Contrato { get; set; } = null!;
    public int InquilinoId { get; set; }
    public Inquilino Inquilino { get; set; } = null!;

    public bool IsPaymentResponsible { get; set; } = false;
}