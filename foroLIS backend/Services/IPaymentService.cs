namespace foroLIS_backend.Services;


using DTOs;

public interface IPaymentService
{
    Task<string> CreatePaymentAsync(CreatePaymentDTO dto);
}
