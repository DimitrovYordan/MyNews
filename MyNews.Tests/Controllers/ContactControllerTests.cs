using Microsoft.AspNetCore.Mvc;

using MyNews.Api.Controllers;
using MyNews.Api.DTOs;
using MyNews.Api.Interfaces;

using Moq;

namespace MyNews.Tests.Controllers
{
    public class ContactControllerTests
    {
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly ContactController _controller;

        public ContactControllerTests()
        {
            _emailServiceMock = new Mock<IEmailService>();
            _controller = new ContactController(_emailServiceMock.Object);
        }

        [Fact]
        public async Task Send_ShouldReturnOk_WhenModelIsValid()
        {
            // Arrange
            var dto = new ContactMessageDto
            {
                Title = "Test",
                FromEmail = "test@example.com",
                Message = "Hello"
            };

            _emailServiceMock
                .Setup(x => x.SendContactMessageAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Send(dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);

            // Deserialize as JsonElement and get boolean
            var json = System.Text.Json.JsonSerializer.Serialize(okResult.Value);
            var data = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, System.Text.Json.JsonElement>>(json);

            Assert.True(data["success"].GetBoolean());

            _emailServiceMock.Verify(s => s.SendContactMessageAsync(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task Send_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Title", "Required");

            // Act
            var result = await _controller.Send(new ContactMessageDto());

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            _emailServiceMock.Verify(s => s.SendContactMessageAsync(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }
    }
}
