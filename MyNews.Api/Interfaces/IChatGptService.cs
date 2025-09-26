using MyNews.Api.Enums;

namespace MyNews.Api.Interfaces
{
    public interface IChatGptService
    {
        Task<List<(string Summary, SectionType Section)>> EnrichNewsBatchAsync(List<string> titles, CancellationToken cancellationToken = default);
    }
}
