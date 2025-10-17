using MyNews.Api.DTOs;

namespace MyNews.Api.Interfaces
{
    public interface IChatGptService
    {
        Task<List<EnrichedNewsDto>> EnrichBatchAsync(List<NewsForEnrichmentDto> items, CancellationToken cancellationToken = default, List<string>? overrideLanguages = null);

        Task<Dictionary<string, NewsTranslationDto>> TranslateTitlesAndSummariesAsync(string title, string summary, List<string> targetLangs, CancellationToken cancellationToken);
    }
}
