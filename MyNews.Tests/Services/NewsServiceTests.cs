using Microsoft.EntityFrameworkCore;

using MyNews.Api.Data;
using MyNews.Api.Enums;
using MyNews.Api.Models;
using MyNews.Api.Services;

namespace MyNews.Tests.Services
{
    public class NewsServiceTests
    {
        private readonly AppDbContext _context;
        private readonly NewsService _service;

        public NewsServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _service = new NewsService(_context);

            SeedData();
        }

        private void SeedData()
        {
            var source = new Source { Id = 1, Name = "BBC", Url = "https://bbc.com" };
            _context.Sources.Add(source);

            _context.NewsItems.AddRange(
                new NewsItem
                {
                    Id = Guid.NewGuid(),
                    Title = "Politics",
                    Section = SectionType.Politics,
                    SourceId = 1,
                    PublishedAt = DateTime.UtcNow.AddHours(-1)
                },
                new NewsItem
                {
                    Id = Guid.NewGuid(),
                    Title = "Sports",
                    Section = SectionType.Sports,
                    SourceId = 1,
                    PublishedAt = DateTime.UtcNow
                });

            _context.SaveChanges();
        }

        //[Fact]
        //public async Task GetNewsBySectionsAndSourcesAsync_ShouldReturnFilteredNews()
        //{
        //    // Act
        //    var result = await _service.GetNewsBySectionsAndSourcesAsync(
        //        new[] { (int)SectionType.Sports },
        //        new[] { 1 });

        //    // Assert
        //    Assert.Single(result);
        //    Assert.Equal("Sports", result.First().Title);
        //}

        //[Fact]
        //public async Task MarkAsReadAsync_ShouldCreateRecord_IfNotExists()
        //{
        //    // Arrange
        //    var userId = Guid.NewGuid();
        //    var newsId = _context.NewsItems.First().Id;

        //    // Act
        //    await _service.MarkAsReadAsync(userId, newsId);

        //    // Assert
        //    var record = _context.UserNewsReads.FirstOrDefault(r => r.UserId == userId && r.NewsItemId == newsId);
        //    Assert.NotNull(record);
        //    Assert.True(record.HasClickedTitle);
        //}

        //[Fact]
        //public async Task MarkAsReadAsync_ShouldUpdateRecord_IfExists()
        //{
        //    // Arrange
        //    var userId = Guid.NewGuid();
        //    var newsId = _context.NewsItems.First().Id;
        //    _context.UserNewsReads.Add(new UserNewsRead
        //    {
        //        UserId = userId,
        //        NewsItemId = newsId,
        //        ReadAt = DateTime.UtcNow.AddHours(-5),
        //        HasClickedTitle = false
        //    });
        //    await _context.SaveChangesAsync();

        //    // Act
        //    await _service.MarkAsReadAsync(userId, newsId);

        //    // Assert
        //    var record = _context.UserNewsReads.First(r => r.UserId == userId && r.NewsItemId == newsId);
        //    Assert.True(record.HasClickedTitle);
        //    Assert.True(record.ReadAt > DateTime.UtcNow.AddMinutes(-1));
        //}

        //[Fact]
        //public async Task MarkLinkClickedAsync_ShouldUpdateRecord_IfExists()
        //{
        //    // Arrange
        //    var userId = Guid.NewGuid();
        //    var newsId = _context.NewsItems.First().Id;
        //    _context.UserNewsReads.Add(new UserNewsRead
        //    {
        //        UserId = userId,
        //        NewsItemId = newsId,
        //        HasClickedLink = false
        //    });
        //    await _context.SaveChangesAsync();

        //    // Act
        //    await _service.MarkLinkClickedAsync(userId, newsId);

        //    // Assert
        //    var record = _context.UserNewsReads.First(r => r.UserId == userId && r.NewsItemId == newsId);
        //    Assert.True(record.HasClickedLink);
        //}

        [Fact]
        public async Task MarkLinkClickedAsync_ShouldDoNothing_IfNoRecord()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var newsId = Guid.NewGuid();

            // Act
            await _service.MarkLinkClickedAsync(userId, newsId);

            // Assert
            Assert.Empty(_context.UserNewsReads.Where(r => r.UserId == userId && r.NewsItemId == newsId));
        }

        //[Fact]
        //public async Task GetNewsBySectionsAsync_ShouldGroupBySection_AndMarkReadFlagsCorrectly()
        //{
        //    // Arrange
        //    var userId = Guid.NewGuid();

        //    var newsPolitics = _context.NewsItems.First(n => n.Section == SectionType.Politics);
        //    var newsSports = _context.NewsItems.First(n => n.Section == SectionType.Sports);

        //    // We mark that the user has read one news item
        //    _context.UserNewsReads.Add(new UserNewsRead
        //    {
        //        UserId = userId,
        //        NewsItemId = newsPolitics.Id,
        //        HasClickedTitle = true,
        //        HasClickedLink = false,
        //        ReadAt = DateTime.UtcNow
        //    });

        //    await _context.SaveChangesAsync();

        //    // Act
        //    var result = await _service.GetNewsBySectionsAsync(
        //        new List<SectionType> { SectionType.Politics, SectionType.Sports },
        //        userId);

        //    // Assert
        //    Assert.NotNull(result);
        //    Assert.Equal(2, result.Count()); // Politics and Sports

        //    var politicsSection = result.First(s => s.SectionId == (int)SectionType.Politics);
        //    var sportsSection = result.First(s => s.SectionId == (int)SectionType.Sports);

        //    // Check for grouping
        //    Assert.Single(politicsSection.News);
        //    Assert.Single(sportsSection.News);

        //    var politicsNews = politicsSection.News.First();
        //    var sportsNews = sportsSection.News.First();

        //    // Politics must be read
        //    Assert.True(politicsNews.IsRead);
        //    Assert.False(politicsNews.IsNew);

        //    // Sports must be new
        //    Assert.False(sportsNews.IsRead);
        //    Assert.True(sportsNews.IsNew);
        //}
    }
}
