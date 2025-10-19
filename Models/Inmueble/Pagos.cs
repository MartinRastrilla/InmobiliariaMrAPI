namespace InmobiliariaMrAPI.Models.Inmueble;

public class Pagos
{
    public int Id { get; set; }
    public int ContratoId { get; set; }
    public Contrato Contrato { get; set; } = null!;
    public int InquilinoId { get; set; }
    public Inquilino Inquilino { get; set; } = null!;
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Cash;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

public enum PaymentMethod
{
    Cash,
    BankTransfer,
    CreditCard,
    DebitCard
}