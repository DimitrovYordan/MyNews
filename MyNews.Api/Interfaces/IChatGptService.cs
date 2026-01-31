using MyNews.Api.DTOs;

namespace MyNews.Api.Interfaces
{
    public interface IChatGptService
    {
        Task<List<EnrichedNewsDto>> EnrichBatchAsync(List<NewsForEnrichmentDto> items, CancellationToken cancellationToken = default, List<string>? overrideLanguages = null);

        Task<Dictionary<string, NewsTranslationDto>> TranslateWithNllbAsync(string title, string summary, string sourceLang, List<string> targetLangs, CancellationToken cancellationToken);
    }
}
