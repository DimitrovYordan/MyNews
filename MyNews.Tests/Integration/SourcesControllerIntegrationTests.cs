using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

using MyNews.Api.Controllers;
using MyNews.Api.Interfaces;
using MyNews.Api.Models;

using Moq;

namespace MyNews.Tests.Integration
{
    /// <summary>
    /// Integration tests for SourcesController.
    /// </summary>
    public class SourcesControllerIntegrationTests
    {
        private static SourcesController CreateController(
            out Mock<ISourceService> sourceMock,
            out Mock<IUserPreferencesService> prefMock,
            bool includeUser = true)
        {
            sourceMock = new Mock<ISourceService>();
            prefMock = new Mock<IUserPreferencesService>();

            var controller = new SourcesController(sourceMock.Object, prefMock.Object);

            var httpContext = new DefaultHttpContext();

            if (includeUser)
            {
                var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()) };
                var identity = new ClaimsIdentity(claims, "TestAuth");
                httpContext.User = new ClaimsPrincipal(identity);
            }

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            return controller;
        }

        /// <summary>
        /// Verify that GetAllSources returns 200 OK and the correct structure.
        /// </summary>
        [Fact]
        public async Task GetAllSources_ShouldReturnOk_WithSelectionFlags()
        {
            // Arrange
            var controller = CreateController(out var sourceMock, out var prefMock);
            sourceMock.Setup(s => s.GetAllSources()).ReturnsAsync(new List<Source>
            {
                new Source { Id = 1, Name = "BBC" },
                new Source { Id = 2, Name = "CNN" }
            });
            prefMock.Setup(p => p.GetSelectedSourcesAsync(It.IsAny<Guid>())).ReturnsAsync(new List<int> { 2 });

            // Act
            var result = await controller.GetAllSources();

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var data = Assert.IsAssignableFrom<IEnumerable<object>>(ok.Value);

            Assert.Collection(data,
                first => Assert.Contains("BBC", first.ToString()),
                second => Assert.Contains("CNN", second.ToString()));
        }

        /// <summary>
        /// Checks that GetAllSources returns Unauthorized when there is no user.
        /// </summary>
        [Fact]
        public async Task GetAllSources_ShouldReturnUnauthorized_WhenNoUser()
        {
            // Arrange
            var controller = CreateController(out _, out _, includeUser: false);

            // Act
            var result = await controller.GetAllSources();

            // Assert
            Assert.IsType<UnauthorizedResult>(result.Result);
        }
    }
}
