namespace foroLIS_backend.DTOs;


public class CreatePaymentDTO
{
    public string Token { get; set; }
    public decimal Amount { get; set; }
    public int Installments { get; set; }
    public string PaymentMethodId { get; set; }
    public string? Email { get; set; } // del donante
    public string ReceiverId { get; set; } // receptor de la donación
    public string? Description { get; set; }
    public Guid PostId { get; set; }
}

