using Xunit;
using Moq;
using foroLIS_backend.Services;
using foroLIS_backend.DTOs;
using foroLIS_backend.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ForoLIS.Tests
{
    public class PaymentsControllerTests
    {
        [Fact]
        public async Task CreatePayment_ReturnsOkResult_WithStatusDetail()
        {
            // Arrange
            var mockService = new Mock<IPaymentService>();
            var expectedStatus = "approved";

            var dto = new CreatePaymentDTO
            {
                Token = "test-token",
                Installments = 1,
                PaymentMethodId = "visa",
                Email = "test@example.com",
                Amount = 100.0m
            };

            mockService.Setup(s => s.CreatePaymentAsync(dto))
                .ReturnsAsync(expectedStatus);

            var controller = new PaymentsController(mockService.Object);

            // Act
            var result = await controller.CreatePayment(dto) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            dynamic value = result.Value;
            Assert.Equal(expectedStatus, value.status);
        }
    }
}

