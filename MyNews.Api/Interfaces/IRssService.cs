using MyNews.Api.DTOs;

namespace MyNews.Api.Interfaces
{
    public interface IRssService
    {
        Task<List<NewsItemDto>> FetchAndProcessRssFeedAsync();
    }
}
