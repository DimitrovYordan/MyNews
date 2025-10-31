using MyNews.Api.Enums;
using MyNews.Api.Services;

namespace MyNews.Tests.Services
{
    public class SectionsServiceTests
    {
        [Fact]
        public async Task GetAllSections_ShouldReturnAllEnumValues()
        {
            // Arrange
            var service = new SectionsService();

            // Act
            var result = await service.GetAllSections();

            // Assert
            var enumValues = Enum.GetValues(typeof(SectionType)).Cast<SectionType>().ToList();

            Assert.NotEmpty(result);
            Assert.Equal(enumValues.Count, result.Count);
            Assert.All(result, s => Assert.False(string.IsNullOrWhiteSpace(s.Name)));
        }
    }
}
