using MyNews.Api.Enums;

namespace MyNews.Api.Interfaces
{
    public interface IUserPreferencesService
    {
        Task<IEnumerable<SectionType>> GetSelectedSectionsAsync(Guid guid);

        Task UpdateSectionsAsync(Guid userId, List<SectionType> sectionIds);
    }
}
