using MyNews.Api.DTOs;
using MyNews.Api.Enums;
using MyNews.Api.Models;

namespace MyNews.Api.Interfaces
{
    public interface INewsService
    {
        Task<IEnumerable<NewsItem>> GetNewsAsync(List<int> sectionsIds);

        Task<IEnumerable<SectionWithNewsDto>> GetNewsBySectionsAsync(List<SectionType> sectionIds);
    }
}
