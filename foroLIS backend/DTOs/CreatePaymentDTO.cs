namespace foroLIS_backend.DTOs;




public class CreatePaymentDTO
{
    public string Token { get; set; }
    public decimal Amount { get; set; }
    public int Installments { get; set; }
    public string PaymentMethodId { get; set; }
    public string? Description { get; set; }
    public Guid PostId { get; set; }
    public string Email { get; set; }

    public CreatePaymentDTO2 toDTO2()
    {
        return new CreatePaymentDTO2
        {
            Amount = this.Amount,
            Description = this.Description,
            Installments = this.Installments,
            PaymentMethodId = this.PaymentMethodId,
            PostId = this.PostId,
            Token = this.Token,
        };
    }
}

public class CreatePaymentDTO2
{
    public string Token { get; set; }
    public decimal Amount { get; set; }
    public int Installments { get; set; }
    public string PaymentMethodId { get; set; }
    public string? Description { get; set; }
    public Guid PostId { get; set; }

    public CreatePaymentDTO toDTO1()
    {
        return new CreatePaymentDTO
        {
            Amount = this.Amount,
            Description = this.Description,
            Installments = this.Installments,
            PaymentMethodId = this.PaymentMethodId,
            PostId = this.PostId,
            Token = this.Token
        };
    }
}

