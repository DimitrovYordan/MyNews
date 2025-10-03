using MyNews.Api.DTOs;

namespace MyNews.Api.Interfaces
{
    public interface IChatGptService
    {
        Task<List<EnrichedNewsDto>> EnrichNewsBatchAsync(List<string> titles, CancellationToken cancellationToken = default);
    }
}
