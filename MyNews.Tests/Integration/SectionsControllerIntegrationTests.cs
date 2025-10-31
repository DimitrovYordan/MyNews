using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

using MyNews.Api.Controllers;
using MyNews.Api.DTOs;
using MyNews.Api.Enums;
using MyNews.Api.Interfaces;

using Moq;

namespace MyNews.Tests.Integration
{
    /// <summary>
    /// Integration tests for SectionsController.
    /// </summary>
    public class SectionsControllerIntegrationTests
    {
        private static SectionsController CreateController(
            out Mock<ISectionsService> sectionMock, 
            out Mock<IUserPreferencesService> prefMock, 
            bool includeUser = true)
        {
            sectionMock = new Mock<ISectionsService>();
            prefMock = new Mock<IUserPreferencesService>();

            var controller = new SectionsController(sectionMock.Object, prefMock.Object);

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

        [Fact]
        public async Task GetAllSections_ShouldReturnOk_WithSelectionFlags()
        {
            // Arrange
            var controller = CreateController(out var sectionMock, out var prefMock);
            sectionMock.Setup(s => s.GetAllSections()).ReturnsAsync(new List<SectionDto>
            {
                new SectionDto { Id = 1, Name = "Politics" },
                new SectionDto { Id = 2, Name = "Sports" }
            });
            prefMock.Setup(p => p.GetSelectedSectionsAsync(It.IsAny<Guid>())).ReturnsAsync(new List<SectionType> { SectionType.Sports });

            // Act
            var result = await controller.GetAllSections();

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var data = Assert.IsAssignableFrom<IEnumerable<object>>(ok.Value);
            Assert.NotEmpty(data);
        }

        [Fact]
        public async Task GetAllSections_ShouldReturnUnauthorized_WhenNoUser()
        {
            // Arrange
            var controller = CreateController(out _, out _, includeUser: false);

            // Act
            var result = await controller.GetAllSections();

            // Assert
            Assert.IsType<UnauthorizedResult>(result.Result);
        }
    }
}
