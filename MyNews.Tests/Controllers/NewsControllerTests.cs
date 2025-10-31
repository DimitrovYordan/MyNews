using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

using MyNews.Api.Controllers;
using MyNews.Api.DTOs;
using MyNews.Api.Enums;
using MyNews.Api.Interfaces;
using MyNews.Api.Models;

using Moq;

namespace MyNews.Tests.Controllers
{
    public class NewsControllerTests
    {
        private readonly Mock<INewsService> _newsServiceMock = new();
        private readonly Mock<IRssService> _rssServiceMock = new();
        private readonly Mock<ISourceService> _sourceServiceMock = new();
        private readonly Mock<IUserPreferencesService> _userPrefsMock = new();
        private readonly NewsController _controller;

        public NewsControllerTests()
        {
            _controller = new NewsController(
                _newsServiceMock.Object,
                _rssServiceMock.Object,
                _sourceServiceMock.Object,
                _userPrefsMock.Object);

            // Setup fake user in HttpContext
            var userId = Guid.NewGuid().ToString();
            var user = new ClaimsPrincipal(new ClaimsIdentity(
                new[] { new Claim(ClaimTypes.NameIdentifier, userId) }, "mock"));
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [Fact]
        public async Task GetNews_ShouldReturnOk_WithValidUser()
        {
            // Arrange
            var sections = new List<int> { 1 };
            var sources = new List<int> { 2 };
            var newsList = new List<NewsItemDto>
            {
                new NewsItemDto { Id = Guid.NewGuid(), Title = "Test", Section = SectionType.Sports }
            };

            _newsServiceMock.Setup(s => s.GetNewsBySectionsAndSourcesAsync(sections, sources))
                            .ReturnsAsync(newsList);

            // Act
            var result = await _controller.GetNews(sections, sources);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var items = Assert.IsAssignableFrom<IEnumerable<object>>(okResult.Value);
            Assert.Single(items);
        }

        [Fact]
        public async Task GetNews_ShouldReturnUnauthorized_WhenUserIdIsMissing()
        {
            // Arrange
            _controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(); // no claims

            // Act
            var result = await _controller.GetNews(new List<int>(), new List<int>());

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task GetRssNews_ShouldReturnOk()
        {
            // Arrange
            var sources = new List<Source>
            {
                new Source { Id = 1, Name = "BBC", Url = "https://bbc.com/rss" }
            };

            var rssNews = new List<NewsItemDto>
            {
                new NewsItemDto { Title = "Rss Test", Link = "https://bbc.com/rss/test" }
            };

            _sourceServiceMock.Setup(s => s.GetAllSources()).ReturnsAsync(sources);
            _rssServiceMock.Setup(r => r.FetchAndProcessRssFeedAsync(sources)).ReturnsAsync(rssNews);

            // Act
            var result = await _controller.GetRssNews();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var items = Assert.IsAssignableFrom<IEnumerable<NewsItemDto>>(okResult.Value);
            Assert.Single(items);
            Assert.Equal("Rss Test", items.First().Title);
        }

        [Fact]
        public async Task MarkAsRead_ShouldReturnOk_WhenValidUser()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            var result = await _controller.MarkAsRead(id);

            // Assert
            Assert.IsType<OkResult>(result);
            _newsServiceMock.Verify(s => s.MarkAsReadAsync(It.IsAny<Guid>(), id), Times.Once);
        }

        [Fact]
        public async Task MarkLinkClicked_ShouldReturnOk_WhenValidUser()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            var result = await _controller.MarkLinkClicked(id);

            // Assert
            Assert.IsType<OkResult>(result);
            _newsServiceMock.Verify(s => s.MarkLinkClickedAsync(It.IsAny<Guid>(), id), Times.Once);
        }

        [Fact]
        public async Task MarkAsRead_ShouldReturnUnauthorized_WhenUserMissing()
        {
            // Arrange
            _controller.ControllerContext.HttpContext.User = new ClaimsPrincipal();

            // Act
            var result = await _controller.MarkAsRead(Guid.NewGuid());

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }
    }
}
