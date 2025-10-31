using Microsoft.AspNetCore.Mvc;

using MyNews.Api.Controllers;
using MyNews.Api.DTOs;
using MyNews.Api.Interfaces;

using Moq;

namespace MyNews.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IAuthService> _mockAuthService;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _mockAuthService = new Mock<IAuthService>();
            _controller = new AuthController(_mockAuthService.Object);
        }

        [Fact]
        public async Task Login_ReturnsOk_WhenCredentialsAreValid()
        {
            // Arrange
            var loginDto = new LoginDto { Email = "test@test.com", Password = "password" };
            var authResponse = new AuthResponseDto { Token = "jwt-token", Email = loginDto.Email };

            _mockAuthService.Setup(x => x.LoginAsync(loginDto)).ReturnsAsync(authResponse);

            // Act
            var result = await _controller.Login(loginDto) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(authResponse, result.Value);
        }

        [Fact]
        public async Task Login_ReturnsUnauthorized_WhenCredentialsAreInvalid()
        {
            // Arrange
            var loginDto = new LoginDto { Email = "wrong@test.com", Password = "1234" };
            _mockAuthService.Setup(x => x.LoginAsync(loginDto)).ReturnsAsync((AuthResponseDto)null);

            // Act
            var result = await _controller.Login(loginDto) as UnauthorizedObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(401, result.StatusCode);
        }

        [Fact]
        public async Task Register_ReturnsOk_WhenRegistrationSucceeds()
        {
            // Arrange
            var registerDto = new RegisterDto { Email = "new@test.com", Password = "pass" };
            var authResponse = new AuthResponseDto { Email = registerDto.Email, Token = "jwt" };

            _mockAuthService.Setup(x => x.RegisterAsync(registerDto)).ReturnsAsync(authResponse);

            // Act
            var result = await _controller.Register(registerDto) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(authResponse, result.Value);
        }

        [Fact]
        public async Task Register_ReturnsBadRequest_WhenUserExists()
        {
            // Arrange
            var registerDto = new RegisterDto { Email = "existing@test.com", Password = "pass" };
            _mockAuthService.Setup(x => x.RegisterAsync(registerDto))
                .ThrowsAsync(new InvalidOperationException("User with this email already exists."));

            // Act
            var result = await _controller.Register(registerDto) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task ForgotPassword_ReturnsOk_WhenEmailExists()
        {
            // Arrange
            var dto = new ForgotPasswordDto { Email = "valid@test.com" };
            _mockAuthService.Setup(x => x.ForgotPasswordAsync(dto.Email)).ReturnsAsync(true);

            // Act
            var result = await _controller.ForgotPassword(dto) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task ForgotPassword_ReturnsBadRequest_WhenEmailNotFound()
        {
            // Arrange
            var dto = new ForgotPasswordDto { Email = "missing@test.com" };
            _mockAuthService.Setup(x => x.ForgotPasswordAsync(dto.Email)).ReturnsAsync(false);

            // Act
            var result = await _controller.ForgotPassword(dto) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task ResetPassword_ReturnsOk_WhenTokenIsValid()
        {
            // Arrange
            var dto = new ResetPasswordDto { Token = "valid-token", NewPassword = "newPass" };
            _mockAuthService.Setup(x => x.ResetPasswordAsync(dto.Token, dto.NewPassword)).ReturnsAsync(true);

            // Act
            var result = await _controller.ResetPasswordAsync(dto) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task ResetPassword_ReturnsBadRequest_WhenTokenInvalid()
        {
            // Arrange
            var dto = new ResetPasswordDto { Token = "invalid-token", NewPassword = "newPass" };
            _mockAuthService.Setup(x => x.ResetPasswordAsync(dto.Token, dto.NewPassword)).ReturnsAsync(false);

            // Act
            var result = await _controller.ResetPasswordAsync(dto) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }
    }
}
