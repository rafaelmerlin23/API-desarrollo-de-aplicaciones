namespace foroLIS_backend.Services;

using MercadoPago.Client.Payment;
using MercadoPago.Resource.Payment;
using DTOs;

public class PaymentService : IPaymentService
{
    public async Task<string> CreatePaymentAsync(CreatePaymentDTO dto)
    {
        var paymentRequest = new PaymentCreateRequest
        {
            TransactionAmount = dto.Amount,
            Token = dto.Token,
            Description = "Compra en el sitio",
            Installments = dto.Installments,
            PaymentMethodId = dto.PaymentMethodId,
            Payer = new PaymentPayerRequest
            {
                Email = dto.Email
            }
        };

        var client = new PaymentClient();
        Payment payment = await client.CreateAsync(paymentRequest);

        return payment.StatusDetail;
    }
}
