using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;
using System.Text.Json;

using MyNews.Api.Controllers;
using MyNews.Api.Interfaces;
using MyNews.Api.Models;

using Moq;

namespace MyNews.Tests.Controllers
{
    public class SourcesControllerTests
    {
        private readonly Mock<ISourceService> _sourceServiceMock;
        private readonly Mock<IUserPreferencesService> _userPreferencesServiceMock;
        private readonly SourcesController _controller;

        public SourcesControllerTests()
        {
            _sourceServiceMock = new Mock<ISourceService>();
            _userPreferencesServiceMock = new Mock<IUserPreferencesService>();

            _controller = new SourcesController(_sourceServiceMock.Object, _userPreferencesServiceMock.Object);

            var userId = Guid.NewGuid().ToString();
            var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, userId) };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(identity) }
            };
        }

        [Fact]
        public async Task GetAllSources_ShouldReturnOk_WithSelectionFlags()
        {
            // Arrange
            var allSources = new List<Source>
            {
                new Source { Id = 1, Name = "BBC", Url = "https://bbc.com" },
                new Source { Id = 2, Name = "CNN", Url = "https://cnn.com" }
            };

            _sourceServiceMock.Setup(s => s.GetAllSources())
                .ReturnsAsync(allSources);

            _userPreferencesServiceMock.Setup(u => u.GetSelectedSourcesAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new List<int> { 1 }); // only BBC is selected

            // Act
            var result = await _controller.GetAllSources();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            // Serialize to read IsSelected securely
            var json = JsonSerializer.Serialize(okResult.Value);
            var list = JsonSerializer.Deserialize<List<JsonElement>>(json);

            Assert.NotNull(list);
            Assert.Equal(2, list.Count);

            // BBC
            Assert.Equal("BBC", list[0].GetProperty("Name").GetString());
            Assert.True(list[0].GetProperty("IsSelected").GetBoolean());

            // CNN
            Assert.Equal("CNN", list[1].GetProperty("Name").GetString());
            Assert.False(list[1].GetProperty("IsSelected").GetBoolean());
        }

        [Fact]
        public async Task GetAllSources_ShouldReturnUnauthorized_WhenUserMissing()
        {
            // Arrange
            _controller.ControllerContext.HttpContext.User = new ClaimsPrincipal();

            // Act
            var result = await _controller.GetAllSources();

            // Assert
            Assert.IsType<UnauthorizedResult>(result.Result);
        }
    }
}
