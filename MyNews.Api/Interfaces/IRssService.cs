using MyNews.Api.DTOs;
using MyNews.Api.Models;

namespace MyNews.Api.Interfaces
{
    public interface IRssService
    {
        Task<List<NewsItemDto>> FetchAndProcessRssFeedAsync(IEnumerable<Source> sources);
    }
}
