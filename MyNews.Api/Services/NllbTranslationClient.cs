using System.Text.Json;

using MyNews.Api.DTOs;

namespace MyNews.Api.Services
{
    public class NllbTranslationClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<NllbTranslationClient> _logger;

        public NllbTranslationClient(HttpClient httpClient, ILogger<NllbTranslationClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<Dictionary<string, NewsTranslationDto>> TranslateAsync(
            string title, string summary, string sourceLang, List<string> targetLangs, CancellationToken cancellationToken = default)
        {
            var request = new
            {
                sourceLang = sourceLang,
                targets = targetLangs,
                title = title,
                summary = summary
            };

            try
            {
                var response = await _httpClient.PostAsJsonAsync("translate", request, cancellationToken);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync(cancellationToken);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var data = JsonSerializer.Deserialize<JsonElement>(json, options);
                var root = data.GetProperty("translations");

                var result = new Dictionary<string, NewsTranslationDto>();

                foreach (var lang in root.EnumerateObject())
                {
                    var titleTr = lang.Value.TryGetProperty("Title", out var tt) ? tt.GetString() : title;
                    var sumTr = lang.Value.TryGetProperty("Summary", out var ss) ? ss.GetString() : summary;

                    result[lang.Name] = new NewsTranslationDto
                    {
                        LanguageCode = lang.Name,
                        Title = titleTr ?? title,
                        Summary = sumTr ?? summary
                    };
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "NLLB translation failed for '{Title}'", title);
                return targetLangs.ToDictionary(l => l.ToUpperInvariant(),
                                               l => new NewsTranslationDto { LanguageCode = l.ToUpperInvariant(), Title = title, Summary = summary });
            }
        }
    }
}
