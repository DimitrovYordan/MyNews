using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MyNews.Api.Controllers;
using MyNews.Api.DTOs;
using MyNews.Api.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyNews.Tests.Controllers
{
    public class UsersControllerTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly UsersController _controller;

        public UsersControllerTests()
        {
            _mockUserService = new Mock<IUserService>();
            _controller = new UsersController(_mockUserService.Object);
        }

        private void SetUser(Guid? userId)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId?.ToString() ?? string.Empty)
            };

            var identity = new ClaimsIdentity(claims, "TestAuth");
            var user = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [Fact]
        public async Task GetProfile_ShouldReturnUnauthorized_WhenUserIdMissing()
        {
            SetUser(null);

            var result = await _controller.GetProfile();

            Assert.IsType<UnauthorizedObjectResult>(result);
            var message = ((UnauthorizedObjectResult)result).Value as string;
            Assert.Equal("Invalid user.", message);
        }

        [Fact]
        public async Task GetProfile_ShouldReturnNotFound_WhenUserNotFound()
        {
            var userId = Guid.NewGuid();
            SetUser(userId);
            _mockUserService.Setup(s => s.GetProfileAsync(userId)).ReturnsAsync((object?)null);

            var result = await _controller.GetProfile();

            Assert.IsType<NotFoundObjectResult>(result);
            var message = ((NotFoundObjectResult)result).Value as string;
            Assert.Equal("User not found.", message);
        }

        [Fact]
        public async Task GetProfile_ShouldReturnOk_WhenUserExists()
        {
            var userId = Guid.NewGuid();
            SetUser(userId);
            var fakeProfile = new { FirstName = "John" };
            _mockUserService.Setup(s => s.GetProfileAsync(userId)).ReturnsAsync(fakeProfile);

            var result = await _controller.GetProfile();

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(fakeProfile, ok.Value);
        }

        [Fact]
        public async Task UpdateCurrentUser_ShouldReturnUnauthorized_WhenInvalidUser()
        {
            SetUser(null);

            var result = await _controller.UpdateCurrentUser(new UpdateUserDto());

            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public async Task UpdateCurrentUser_ShouldReturnNotFound_WhenUserNotExists()
        {
            var userId = Guid.NewGuid();
            SetUser(userId);
            _mockUserService.Setup(s => s.UpdateProfileAsync(userId, It.IsAny<UpdateUserDto>())).ReturnsAsync((string?)null);

            var result = await _controller.UpdateCurrentUser(new UpdateUserDto());

            Assert.IsType<NotFoundObjectResult>(result);
            var message = ((NotFoundObjectResult)result).Value as string;
            Assert.Equal("User not found.", message);
        }

        [Fact]
        public async Task UpdateCurrentUser_ShouldReturnBadRequest_WhenPasswordsDoNotMatch()
        {
            var userId = Guid.NewGuid();
            SetUser(userId);
            _mockUserService.Setup(s => s.UpdateProfileAsync(userId, It.IsAny<UpdateUserDto>()))
                .ReturnsAsync("Passwords do not match.");

            var result = await _controller.UpdateCurrentUser(new UpdateUserDto());

            var badReq = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Passwords do not match.", badReq.Value);
        }

        [Fact]
        public async Task UpdateCurrentUser_ShouldReturnOk_WhenUpdateSuccessful()
        {
            var userId = Guid.NewGuid();
            SetUser(userId);
            _mockUserService.Setup(s => s.UpdateProfileAsync(userId, It.IsAny<UpdateUserDto>()))
                .ReturnsAsync("Profile updated successfully.");

            var result = await _controller.UpdateCurrentUser(new UpdateUserDto());

            var ok = Assert.IsType<OkObjectResult>(result);
            var response = ok.Value.GetType().GetProperty("message")?.GetValue(ok.Value, null);
            Assert.Equal("Profile updated successfully.", response);
        }

        [Fact]
        public async Task DeleteCurrentUser_ShouldReturnUnauthorized_WhenInvalidUser()
        {
            SetUser(null);

            var result = await _controller.DeleteCurrentUser();

            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public async Task DeleteCurrentUser_ShouldReturnNotFound_WhenUserNotFound()
        {
            var userId = Guid.NewGuid();
            SetUser(userId);
            _mockUserService.Setup(s => s.DeleteUserAsync(userId)).ReturnsAsync((string?)null);

            var result = await _controller.DeleteCurrentUser();

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("User not found.", notFound.Value);
        }

        [Fact]
        public async Task DeleteCurrentUser_ShouldReturnOk_WhenUserDeleted()
        {
            var userId = Guid.NewGuid();
            SetUser(userId);
            _mockUserService.Setup(s => s.DeleteUserAsync(userId)).ReturnsAsync("User deleted successfully.");

            var result = await _controller.DeleteCurrentUser();

            var ok = Assert.IsType<OkObjectResult>(result);
            var message = ok.Value.GetType().GetProperty("message")?.GetValue(ok.Value, null);
            Assert.Equal("User deleted successfully.", message);
        }

        [Fact]
        public async Task GetOnboardingStatus_ShouldReturnUnauthorized_WhenInvalidUser()
        {
            SetUser(null);

            var result = await _controller.GetOnboardingStatus();

            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public async Task GetOnboardingStatus_ShouldReturnNotFound_WhenUserMissing()
        {
            var userId = Guid.NewGuid();
            SetUser(userId);
            _mockUserService.Setup(s => s.GetOnboardingStatusAsync(userId)).ReturnsAsync((bool?)null);

            var result = await _controller.GetOnboardingStatus();

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("User not found.", notFound.Value);
        }

        [Fact]
        public async Task GetOnboardingStatus_ShouldReturnOk_WhenUserFound()
        {
            var userId = Guid.NewGuid();
            SetUser(userId);
            _mockUserService.Setup(s => s.GetOnboardingStatusAsync(userId)).ReturnsAsync(true);

            var result = await _controller.GetOnboardingStatus();

            var ok = Assert.IsType<OkObjectResult>(result);
            var status = ok.Value.GetType().GetProperty("isOnboardingComplete")?.GetValue(ok.Value, null);
            Assert.True((bool)status);
        }

        [Fact]
        public async Task CompleteOnboarding_ShouldReturnUnauthorized_WhenInvalidUser()
        {
            SetUser(null);

            var result = await _controller.CompleteOnboarding();

            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public async Task CompleteOnboarding_ShouldReturnNotFound_WhenUserNotFound()
        {
            var userId = Guid.NewGuid();
            SetUser(userId);
            _mockUserService.Setup(s => s.MarkOnboardingCompletedAsync(userId)).ReturnsAsync(false);

            var result = await _controller.CompleteOnboarding();

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("User not found.", notFound.Value);
        }

        [Fact]
        public async Task CompleteOnboarding_ShouldReturnOk_WhenSuccess()
        {
            var userId = Guid.NewGuid();
            SetUser(userId);
            _mockUserService.Setup(s => s.MarkOnboardingCompletedAsync(userId)).ReturnsAsync(true);

            var result = await _controller.CompleteOnboarding();

            var ok = Assert.IsType<OkObjectResult>(result);
            var message = ok.Value.GetType().GetProperty("message")?.GetValue(ok.Value, null);
            Assert.Equal("Onboarding marked as completed.", message);
        }
    }
}
