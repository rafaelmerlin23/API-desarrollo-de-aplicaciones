using foroLIS_backend.Controllers;
using foroLIS_backend.DTOs;
using foroLIS_backend.Models;
using foroLIS_backend.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace ForoLIS.Tests
{
    public class PaymentsControllerTests
    {
        [Fact]
        public async Task CreatePayment_ReturnsOkResult_WithStatusDetail()
        {
            // Arrange
            var mockService = new Mock<IPaymentService>();
            var mockUserManager = new Mock<UserManager<Users>>(
                new Mock<IUserStore<Users>>().Object,
                null, null, null, null, null, null, null, null
            );
            var mockCurrentUser = new Mock<ICurrentUserService>();

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

            var controller = new PaymentsController(
                mockService.Object,
                mockUserManager.Object,
                mockCurrentUser.Object
            );

            // Act
            var result = await controller.CreatePayment(dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            dynamic value = okResult.Value;
            Assert.Equal(expectedStatus, (string)value.status);
        }

    }
}

