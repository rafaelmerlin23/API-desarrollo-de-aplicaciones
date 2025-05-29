namespace foroLIS_backend.Controllers;

// Controllers/PaymentsController.cs
using Microsoft.AspNetCore.Mvc;
using DTOs;
using Services;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentsController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentDTO dto)
    {
        var result = await _paymentService.CreatePaymentAsync(dto);
        return Ok(new { status = result });
    }
}
