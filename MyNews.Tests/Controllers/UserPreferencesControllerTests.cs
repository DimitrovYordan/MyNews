using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

using MyNews.Api.Controllers;
using MyNews.Api.Enums;
using MyNews.Api.Interfaces;

using Moq;

namespace MyNews.Tests.Controllers
{
    public class UserPreferencesControllerTests
    {
        private readonly Mock<IUserPreferencesService> _mockService;
        private readonly UserPreferencesController _controller;
        private readonly Guid _testUserId = Guid.NewGuid();

        public UserPreferencesControllerTests()
        {
            _mockService = new Mock<IUserPreferencesService>();
            _controller = new UserPreferencesController(_mockService.Object);

            // We set the context to contain User claim with Guid
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, _testUserId.ToString())
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        // ---------- [GET] /api/userpreferences/sections ----------
        [Fact]
        public async Task GetSelectedSections_ShouldReturnOk_WithUserSections()
        {
            // Arrange
            var expectedSections = new List<SectionType> { SectionType.Politics, SectionType.World_News };
            _mockService.Setup(s => s.GetSelectedSectionsAsync(_testUserId))
                        .ReturnsAsync(expectedSections);

            // Act
            var result = await _controller.GetSelectedSections();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var sections = Assert.IsAssignableFrom<IEnumerable<SectionType>>(okResult.Value);
            Assert.Equal(expectedSections, sections);
        }

        [Fact]
        public async Task GetSelectedSections_ShouldReturnUnauthorized_WhenUserMissing()
        {
            // Arrange
            _controller.ControllerContext.HttpContext.User = new ClaimsPrincipal();

            // Act
            var result = await _controller.GetSelectedSections();

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        // ---------- [POST] /api/userpreferences/sections ----------
        [Fact]
        public async Task UpdateSections_ShouldReturnOk_WhenCalled()
        {
            // Arrange
            var sections = new List<SectionType> { SectionType.Business };
            _mockService.Setup(s => s.UpdateSectionsAsync(_testUserId, sections))
                        .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateSections(sections);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = okResult.Value;

            // reflection
            var messageProp = value.GetType().GetProperty("message");
            Assert.NotNull(messageProp);

            var messageValue = messageProp.GetValue(value)?.ToString();
            Assert.Equal("Sections updated.", messageValue);

            _mockService.Verify(s => s.UpdateSectionsAsync(_testUserId, sections), Times.Once);
        }

        [Fact]
        public async Task UpdateSections_ShouldReturnUnauthorized_WhenNoUser()
        {
            _controller.ControllerContext.HttpContext.User = new ClaimsPrincipal();

            var result = await _controller.UpdateSections(new List<SectionType>());

            Assert.IsType<UnauthorizedResult>(result);
        }

        // ---------- [GET] /api/userpreferences/sources ----------
        [Fact]
        public async Task GetSelectedSources_ShouldReturnOk_WithSourceIds()
        {
            // Arrange
            var expectedSources = new List<int> { 1, 2, 3 };
            _mockService.Setup(s => s.GetSelectedSourcesAsync(_testUserId))
                        .ReturnsAsync(expectedSources);

            // Act
            var result = await _controller.GetSelectedSources();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var sources = Assert.IsAssignableFrom<IEnumerable<int>>(okResult.Value);
            Assert.Equal(expectedSources, sources);
        }

        [Fact]
        public async Task GetSelectedSources_ShouldReturnUnauthorized_WhenUserMissing()
        {
            _controller.ControllerContext.HttpContext.User = new ClaimsPrincipal();

            var result = await _controller.GetSelectedSources();

            Assert.IsType<UnauthorizedResult>(result);
        }

        // ---------- [POST] /api/userpreferences/sources ----------
        [Fact]
        public async Task UpdateSources_ShouldReturnOk_WhenValid()
        {
            // Arrange
            var sourceIds = new List<int> { 5, 6 };
            _mockService.Setup(s => s.UpdateSourcesAsync(_testUserId, sourceIds))
                        .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateSources(sourceIds);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = okResult.Value;

            // reflection
            var messageProp = value.GetType().GetProperty("message");
            Assert.NotNull(messageProp);

            var messageValue = messageProp.GetValue(value)?.ToString();
            Assert.Equal("Sources updated.", messageValue);

            _mockService.Verify(s => s.UpdateSourcesAsync(_testUserId, sourceIds), Times.Once);
        }

        [Fact]
        public async Task UpdateSources_ShouldReturnUnauthorized_WhenNoUser()
        {
            _controller.ControllerContext.HttpContext.User = new ClaimsPrincipal();

            var result = await _controller.UpdateSources(new List<int>());

            Assert.IsType<UnauthorizedResult>(result);
        }
    }
}
