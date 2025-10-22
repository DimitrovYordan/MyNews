using MyNews.Api.DTOs;
using MyNews.Api.Enums;

namespace MyNews.Api.Interfaces
{
    public interface INewsService
    {
        Task<List<NewsItemDto>> GetNewsBySectionsAndSourcesAsync(IEnumerable<int> sectionIds, IEnumerable<int> sourceIds);

        Task<IEnumerable<SectionWithNewsDto>> GetNewsBySectionsAsync(List<SectionType> sectionIds, Guid userId);

        Task MarkAsReadAsync(Guid userId, Guid newsItemId);

        Task MarkLinkClickedAsync(Guid userId, Guid newsItemId);
    }
}
