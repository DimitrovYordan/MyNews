using Microsoft.EntityFrameworkCore;

using MyNews.Api.Data;
using MyNews.Api.Enums;
using MyNews.Api.Models;
using MyNews.Api.Services;

namespace MyNews.Tests.Services
{
    public class UserPreferencesServiceTests
    {
        private readonly DbContextOptions<AppDbContext> _dbOptions;

        public UserPreferencesServiceTests()
        {
            _dbOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: $"UserPrefsDb_{Guid.NewGuid()}")
                .Options;
        }

        //[Fact]
        //public async Task GetSelectedSectionsAsync_ShouldReturnOnlySelectedSections()
        //{
        //    // Arrange
        //    var userId = Guid.NewGuid();
        //    using (var context = new AppDbContext(_dbOptions))
        //    {
        //        context.UserSectionPreference.AddRange(
        //            new UserSectionPreference { UserId = userId, SectionType = SectionType.Politics, IsSelected = true },
        //            new UserSectionPreference { UserId = userId, SectionType = SectionType.Sports, IsSelected = false }
        //        );
        //        await context.SaveChangesAsync();
        //    }

        //    // Act
        //    using (var context = new AppDbContext(_dbOptions))
        //    {
        //        var service = new UserPreferencesService(context);
        //        var result = await service.GetSelectedSectionsAsync(userId);

        //        // Assert
        //        Assert.Single(result);
        //        Assert.Contains(SectionType.Politics, result);
        //    }
        //}

        //[Fact]
        //public async Task UpdateSectionsAsync_ShouldReplaceExistingPreferences()
        //{
        //    // Arrange
        //    var userId = Guid.NewGuid();

        //    using (var context = new AppDbContext(_dbOptions))
        //    {
        //        context.UserSectionPreference.AddRange(
        //            new UserSectionPreference { UserId = userId, SectionType = SectionType.Business, IsSelected = true }
        //        );
        //        await context.SaveChangesAsync();
        //    }

        //    // Act
        //    using (var context = new AppDbContext(_dbOptions))
        //    {
        //        var service = new UserPreferencesService(context);
        //        await service.UpdateSectionsAsync(userId, new List<SectionType> { SectionType.Technology, SectionType.World_News });
        //    }

        //    // Assert
        //    using (var context = new AppDbContext(_dbOptions))
        //    {
        //        var all = await context.UserSectionPreference.Where(x => x.UserId == userId).ToListAsync();
        //        Assert.Equal(2, all.Count);
        //        Assert.DoesNotContain(all, x => x.SectionType == SectionType.Business);
        //    }
        //}

        //[Fact]
        //public async Task GetSelectedSourcesAsync_ShouldReturnOnlySelectedSources()
        //{
        //    // Arrange
        //    var userId = Guid.NewGuid();
        //    using (var context = new AppDbContext(_dbOptions))
        //    {
        //        context.UserSourcePreferences.AddRange(
        //            new UserSourcePreferences { UserId = userId, SourceId = 1, IsSelected = true },
        //            new UserSourcePreferences { UserId = userId, SourceId = 2, IsSelected = false }
        //        );
        //        await context.SaveChangesAsync();
        //    }

        //    // Act
        //    using (var context = new AppDbContext(_dbOptions))
        //    {
        //        var service = new UserPreferencesService(context);
        //        var result = await service.GetSelectedSourcesAsync(userId);

        //        // Assert
        //        Assert.Single(result);
        //        Assert.Contains(1, result);
        //    }
        //}

        //[Fact]
        //public async Task UpdateSourcesAsync_ShouldReplaceExistingSourcePreferences()
        //{
        //    // Arrange
        //    var userId = Guid.NewGuid();
        //    using (var context = new AppDbContext(_dbOptions))
        //    {
        //        context.UserSourcePreferences.Add(new UserSourcePreferences
        //        {
        //            UserId = userId,
        //            SourceId = 1,
        //            IsSelected = true
        //        });
        //        await context.SaveChangesAsync();
        //    }

        //    // Act
        //    using (var context = new AppDbContext(_dbOptions))
        //    {
        //        var service = new UserPreferencesService(context);
        //        await service.UpdateSourcesAsync(userId, new List<int> { 3, 4 });
        //    }

        //    // Assert
        //    using (var context = new AppDbContext(_dbOptions))
        //    {
        //        var prefs = await context.UserSourcePreferences.Where(x => x.UserId == userId).ToListAsync();
        //        Assert.Equal(2, prefs.Count);
        //        Assert.DoesNotContain(prefs, x => x.SourceId == 1);
        //    }
        //}
    }
}
