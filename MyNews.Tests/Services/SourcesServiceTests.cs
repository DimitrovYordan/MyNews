using Microsoft.EntityFrameworkCore;

using MyNews.Api.Data;
using MyNews.Api.Models;
using MyNews.Api.Services;

namespace MyNews.Tests.Services
{
    public class SourcesServiceTests
    {
        private readonly DbContextOptions<AppDbContext> _dbOptions;

        public SourcesServiceTests()
        {
            _dbOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: $"SourcesServiceTests_{System.Guid.NewGuid()}")
                .Options;
        }

        [Fact]
        public async Task GetAllSources_ShouldReturnAllSourcesFromDatabase()
        {
            // Arrange
            using (var context = new AppDbContext(_dbOptions))
            {
                context.Sources.AddRange(new List<Source>
                {
                    new Source { Id = 1, Name = "BBC", Url = "https://bbc.com" },
                    new Source { Id = 2, Name = "CNN", Url = "https://cnn.com" }
                });
                await context.SaveChangesAsync();
            }

            // Act
            using (var context = new AppDbContext(_dbOptions))
            {
                var service = new SourcesService(context);
                var result = await service.GetAllSources();

                // Assert
                Assert.NotNull(result);
                Assert.Equal(2, result.Count);

                var bbc = result.FirstOrDefault(s => s.Id == 1);
                var cnn = result.FirstOrDefault(s => s.Id == 2);

                Assert.Equal("BBC", bbc.Name);
                Assert.Equal("https://bbc.com", bbc.Url);

                Assert.Equal("CNN", cnn.Name);
                Assert.Equal("https://cnn.com", cnn.Url);
            }
        }

        [Fact]
        public async Task GetAllSources_ShouldReturnEmptyList_WhenNoSourcesExist()
        {
            // Arrange
            using var context = new AppDbContext(_dbOptions);
            var service = new SourcesService(context);

            // Act
            var result = await service.GetAllSources();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
