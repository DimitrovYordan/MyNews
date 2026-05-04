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
                var data = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json, options);

                var result = new Dictionary<string, NewsTranslationDto>();

                foreach (var kvp in data)
                {
                    var langCode = kvp.Key;
                    var arr = kvp.Value;

                    if (arr.ValueKind == JsonValueKind.Array && arr.GetArrayLength() > 0)
                    {
                        var first = arr[0];

                        var titleTr = first.TryGetProperty("Title", out var tt) ? tt.GetString() : title;
                        var sumTr = first.TryGetProperty("Summary", out var ss) ? ss.GetString() : summary;

                        result[langCode] = new NewsTranslationDto
                        {
                            LanguageCode = langCode,
                            Title = titleTr ?? title,
                            Summary = sumTr ?? summary
                        };
                    }
                }

                if (!result.Any())
                {
                    foreach (var target in targetLangs)
                    {
                        result[target] = new NewsTranslationDto
                        {
                            LanguageCode = target,
                            Title = title,
                            Summary = summary
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

                return targetLangs.ToDictionary(l => l,
                    l => new NewsTranslationDto { LanguageCode = l, Title = title, Summary = summary });
            }
        }
    }
}
