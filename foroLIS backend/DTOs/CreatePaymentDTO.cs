namespace foroLIS_backend.DTOs;


public class CreatePaymentDTO
{
    public string Token { get; set; }
    public int Installments { get; set; }
    public string PaymentMethodId { get; set; }
    public string Email { get; set; }
    public decimal Amount { get; set; }
}
