using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Security.Claims;

using MyNews.Api.Controllers;
using MyNews.Api.Data;
using MyNews.Api.Enums;
using MyNews.Api.Services;

namespace MyNews.Tests.Integration
{
    public class UserPreferencesControllerLightTests
    {
        private static UserPreferencesController CreateController(out AppDbContext dbContext, Guid? userId = null)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            dbContext = new AppDbContext(options);

            var service = new UserPreferencesService(dbContext);
            var controller = new UserPreferencesController(service);

            // We simulate a user with a userId (if none — it will return Unauthorized)
            if (userId.HasValue)
            {
                var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId.Value.ToString())
                }, "mock"));

                controller.ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = user }
                };
            }

            return controller;
        }

        //[Fact]
        //public async Task GetSelectedSections_ShouldReturnOk_WhenUserExists()
        //{
        //    // Arrange
        //    var userId = Guid.NewGuid();
        //    var controller = CreateController(out var db, userId);

        //    db.UserSectionPreference.Add(new()
        //    {
        //        UserId = userId,
        //        SectionType = SectionType.Sports,
        //        IsSelected = true
        //    });
        //    await db.SaveChangesAsync();

        //    // Act
        //    var result = await controller.GetSelectedSections();

        //    // Assert
        //    var okResult = Assert.IsType<OkObjectResult>(result);
        //    var sections = Assert.IsAssignableFrom<IEnumerable<SectionType>>(okResult.Value);
        //    Assert.Contains(SectionType.Sports, sections);
        //}

        /// <summary>
        /// Confirms that UpdateSections successfully updates user section preferences
        /// and returns a success message when the request is valid.
        /// </summary>
        //[Fact]
        //public async Task UpdateSections_ShouldReturnOk_AndPersistChanges()
        //{
        //    // Arrange
        //    var userId = Guid.NewGuid();
        //    var controller = CreateController(out var db, userId);
        //    var sections = new List<SectionType> { SectionType.Business, SectionType.Technology };

        //    // Act
        //    var result = await controller.UpdateSections(sections);

        //    // Assert
        //    var ok = Assert.IsType<OkObjectResult>(result);
        //    var json = ok.Value!.ToString();
        //    Assert.Contains("Sections updated", json);

        //    var prefs = await db.UserSectionPreference.ToListAsync();
        //    Assert.Equal(2, prefs.Count);
        //}

        //[Fact]
        //public async Task GetSelectedSources_ShouldReturnOk_WhenUserExists()
        //{
        //    // Arrange
        //    var userId = Guid.NewGuid();
        //    var controller = CreateController(out var db, userId);

        //    db.UserSourcePreferences.Add(new()
        //    {
        //        UserId = userId,
        //        SourceId = 42,
        //        IsSelected = true
        //    });
        //    await db.SaveChangesAsync();

        //    // Act
        //    var result = await controller.GetSelectedSources();

        //    // Assert
        //    var okResult = Assert.IsType<OkObjectResult>(result);
        //    var sources = Assert.IsAssignableFrom<IEnumerable<int>>(okResult.Value);
        //    Assert.Contains(42, sources);
        //}

        /// <summary>
        /// Confirms that UpdateSources updates user source preferences correctly
        /// and returns a success message when data is valid.
        /// </summary>
        //[Fact]
        //public async Task UpdateSources_ShouldReturnOk_AndPersistChanges()
        //{
        //    // Arrange
        //    var userId = Guid.NewGuid();
        //    var controller = CreateController(out var db, userId);
        //    var sourceIds = new List<int> { 1, 2, 3 };

        //    // Act
        //    var result = await controller.UpdateSources(sourceIds);

        //    // Assert
        //    var ok = Assert.IsType<OkObjectResult>(result);

        //    var json = ok.Value!.ToString();
        //    Assert.Contains("Sources updated", json);

        //    var prefs = await db.UserSourcePreferences.ToListAsync();
        //    Assert.Equal(3, prefs.Count);
        //}

        [Fact]
        public async Task GetSelectedSections_ShouldReturnUnauthorized_WhenNoUser()
        {
            // Arrange
            var controller = CreateController(out _);

            // Act
            var result = await controller.GetSelectedSections();

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }
    }
}
