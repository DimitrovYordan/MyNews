using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;
using System.Text.Json;

using MyNews.Api.Controllers;
using MyNews.Api.DTOs;
using MyNews.Api.Enums;
using MyNews.Api.Interfaces;

using Moq;

namespace MyNews.Tests.Controllers
{
    public class SectionsControllerTests
    {
        private readonly Mock<ISectionsService> _sectionsServiceMock;
        private readonly Mock<IUserPreferencesService> _userPreferencesServiceMock;
        private readonly SectionsController _controller;

        public SectionsControllerTests()
        {
            _sectionsServiceMock = new Mock<ISectionsService>();
            _userPreferencesServiceMock = new Mock<IUserPreferencesService>();

            _controller = new SectionsController(_sectionsServiceMock.Object, _userPreferencesServiceMock.Object);

            var userId = Guid.NewGuid().ToString();
            var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, userId) };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(identity) }
            };
        }

        [Fact]
        public async Task GetAllSections_ShouldReturnOk_WithSelectionFlags()
        {
            // Arrange
            var allSections = new List<SectionDto>
            {
                new SectionDto { Id = 1, Name = "Politics" },
                new SectionDto { Id = 2, Name = "Sports" }
            };

            _sectionsServiceMock.Setup(s => s.GetAllSections())
                .ReturnsAsync(allSections);

            _userPreferencesServiceMock.Setup(u => u.GetSelectedSectionsAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new List<SectionType> { (SectionType)1 });

            // Act
            var result = await _controller.GetAllSections();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            // Serialize and return as a JSON element for robust access to properties
            var json = JsonSerializer.Serialize(okResult.Value);
            var list = JsonSerializer.Deserialize<List<JsonElement>>(json);

            Assert.NotNull(list);
            Assert.Equal(2, list.Count);

            // Checking values
            Assert.Equal("Politics", list[0].GetProperty("Name").GetString());
            Assert.True(list[0].GetProperty("IsSelected").GetBoolean());

            Assert.Equal("Sports", list[1].GetProperty("Name").GetString());
            Assert.False(list[1].GetProperty("IsSelected").GetBoolean());
        }

        [Fact]
        public async Task GetAllSections_ShouldReturnUnauthorized_WhenUserMissing()
        {
            // Arrange
            _controller.ControllerContext.HttpContext.User = new ClaimsPrincipal();

            // Act
            var result = await _controller.GetAllSections();

            // Assert
            Assert.IsType<UnauthorizedResult>(result.Result);
        }
    }
}
