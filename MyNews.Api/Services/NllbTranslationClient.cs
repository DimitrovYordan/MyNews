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
                summary = summary,
                items = new[]
                {
                    new
                    {
                        title = title,
                        summary = summary
                    }
                }
            };

            _logger.LogInformation("[NLLB REQUEST] SourceLang={SourceLang}, Targets={Targets}, Title={Title}, SummaryLength={SummaryLength}",
                sourceLang,
                string.Join(",", targetLangs),
                title,
                summary?.Length ?? 0
            );

            try
            {
                var response = await _httpClient.PostAsJsonAsync("translate", request, cancellationToken);
                _logger.LogInformation("[NLLB RESPONSE STATUS] {StatusCode} for '{Title}'", response.StatusCode, title);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync(cancellationToken);
                _logger.LogInformation("[NLLB RESPONSE RAW] {Json}", json.Length > 1000 ? json.Substring(0, 1000) + "..." : json);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var data = JsonSerializer.Deserialize<JsonElement>(json, options);
                var root = data.GetProperty("translations");

                var result = new Dictionary<string, NewsTranslationDto>();

                foreach (var lang in root.EnumerateObject())
                {
                    var arr = lang.Value;

                    if (arr.ValueKind == JsonValueKind.Array && arr.GetArrayLength() > 0)
                    {
                        var first = arr[0];

                        var titleTr = first.TryGetProperty("Title", out var tt) ? tt.GetString() : title;
                        var sumTr = first.TryGetProperty("Summary", out var ss) ? ss.GetString() : summary;

                        result[lang.Name] = new NewsTranslationDto
                        {
                            LanguageCode = lang.Name,
                            Title = titleTr ?? title,
                            Summary = sumTr ?? summary
                        };
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "[NLLB ERROR] Failed for Title='{Title}', SourceLang={SourceLang}, Targets={Targets}",
                    title,
                    sourceLang,
                    string.Join(",", targetLangs)
                );
                return targetLangs.ToDictionary(l => l.ToUpperInvariant(),
                                               l => new NewsTranslationDto { LanguageCode = l.ToUpperInvariant(), Title = title, Summary = summary });
            }
        }
    }
}
