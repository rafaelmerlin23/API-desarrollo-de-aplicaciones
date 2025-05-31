using foroLIS_backend.DTOs;
using foroLIS_backend.Models;
using foroLIS_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace foroLIS_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly UserManager<Users> _userManager;
        private readonly ICurrentUserService _currentUserService;

        public PaymentsController(
            IPaymentService paymentService,
            UserManager<Users> userManager,
            ICurrentUserService currentUserService)
        {
            _paymentService = paymentService;
            _userManager = userManager;
            _currentUserService = currentUserService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentDTO dto)
        {
            var result = await _paymentService.CreatePaymentAsync(dto);
            return Ok(new { status = result });
        }

 

    }
}
