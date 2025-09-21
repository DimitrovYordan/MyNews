using MyNews.Api.DTOs;
using MyNews.Api.Enums;
using MyNews.Api.Models;

namespace MyNews.Api.Interfaces
{
    public interface INewsService
    {
        Task<IEnumerable<NewsItem>> GetNewsAsync(List<int> sectionsIds);

        Task<IEnumerable<SectionWithNewsDto>> GetNewsBySectionsAsync(List<SectionType> sectionIds);

        Task<bool> ExistsByTitleAndSourceAsync(string title, int sourceId);

        Task<NewsItem> AddNewsItemAsync(NewsItem newsItem);

        Task MarkAsReadAsync(int userId, Guid newsItemId);

        Task<bool> IsReadAsync(int userId, Guid newsItemId);
    }
}
