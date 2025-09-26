using System.Net.Http.Headers;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using MyNews.Api.Enums;
using MyNews.Api.Interfaces;
using Newtonsoft.Json;

namespace MyNews.Api.Services
{
    public class ChatGptService : IChatGptService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly ILogger<ChatGptService> _logger;
        private readonly int _batchChunkSize = 1;

        public ChatGptService(HttpClient httpClient, IConfiguration configuration, ILogger<ChatGptService> logger)
        {
            _httpClient = httpClient;
            _apiKey = configuration["OpenAI:ApiKey"];
            _logger = logger;
        }

        public async Task<List<(string Summary, SectionType Section)>> EnrichNewsBatchAsync(
            List<string> titles, CancellationToken cancellationToken = default)
        {
            var results = new List<(string Summary, SectionType Section)>();

            for (int i = 0; i < titles.Count; i += _batchChunkSize)
            {
                var chunk = titles.Skip(i).Take(_batchChunkSize).ToList();

                var sectionTypes = string.Join(", ", Enum.GetNames(typeof(SectionType)));
                var systemMessage = $"You summarize multiple news titles in few sentences each and assign them to a section from the following options: {sectionTypes}. " +
                                    $"Return a valid JSON array where each item is {{ \"Summary\": \"...\", \"Section\": \"...\" }}. " +
                                    $"The order must match the input titles.";

                var joinedTitles = string.Join("\n", chunk.Select((t, idx) =>
                {
                    var safeTitle = t.Replace("\"", "\\\"").Replace("\n", " ").Replace("\r", " ");
                    return $"{idx + 1}. {safeTitle}";
                }));

                var messages = new[]
                {
                    new { role = "system", content = systemMessage },
                    new { role = "user", content = $"Here are the titles:\n{joinedTitles}" }
                };

                var requestBodyObj = new
                {
                    model = "gpt-3.5-turbo",
                    messages = messages,
                    max_tokens = 400
                };

                var contentStr = await SendChatRequestAsync(requestBodyObj, 400, cancellationToken);

                if (string.IsNullOrWhiteSpace(contentStr))
                {
                    results.AddRange(chunk.Select(_ => ("Summary unavailable", SectionType.General)));
                    continue;
                }

                try
                {
                    var json = JsonDocument.Parse(contentStr);

                    foreach (var item in json.RootElement.EnumerateArray())
                    {
                        var summary = item.GetProperty("Summary").GetString() ?? string.Empty;
                        var sectionStr = item.GetProperty("Section").GetString() ?? string.Empty;

                        if (!Enum.TryParse(sectionStr.Trim(), out SectionType section))
                        {
                            section = SectionType.General;
                        }

                        results.Add((summary, section));
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to parse GPT JSON for batch news");
                    results.AddRange(chunk.Select(_ => ("Summary unavailable", SectionType.General)));
                }
            }

            return results;
        }

        private async Task<string?> SendChatRequestAsync(object requestBody, int maxToken, CancellationToken cancellationToken = default)
        {
            var requestJson = JsonConvert.SerializeObject(requestBody);


            var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

            _logger.LogInformation("ChatGPT request JSON: {Json}", requestJson);

            const int maxRetries = 1;
            int delaySeconds = 5;

            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                try
                {
                    var response = await _httpClient.PostAsync(
                        "https://api.openai.com/v1/chat/completions",
                        requestContent,
                        cancellationToken);

                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsStringAsync(cancellationToken);
                    }

                    if ((int)response.StatusCode == 429)
                    {
                        _logger.LogWarning(
                            "Rate limited by OpenAI API (429). Attempt {Attempt}/{MaxRetries}. Retrying in {Delay}s...",
                            attempt, maxRetries, delaySeconds);

                        await Task.Delay(TimeSpan.FromSeconds(delaySeconds), cancellationToken);
                        delaySeconds *= 2;
                        var raw = await response.Content.ReadAsStringAsync(cancellationToken);
                        _logger.LogWarning("OpenAI raw response: {Raw}", raw);
                        continue;
                    }

                    response.EnsureSuccessStatusCode();
                }
                catch (HttpRequestException ex)
                {
                    _logger.LogError(ex, "HTTP error during request to OpenAI. Attempt {Attempt}/{MaxRetries}", attempt, maxRetries);
                    if (attempt == maxRetries) throw;
                    await Task.Delay(TimeSpan.FromSeconds(delaySeconds), cancellationToken);
                    delaySeconds *= 2;
                }
            }

            return null;
        }
    }
}
