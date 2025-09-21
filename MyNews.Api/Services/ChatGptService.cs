using MyNews.Api.Interfaces;

using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace MyNews.Api.Services
{
    public class ChatGptService : IChatGptService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly ILogger<ChatGptService> _logger;

        public ChatGptService(HttpClient httpClient, IConfiguration configuration, ILogger<ChatGptService> logger)
        {
            _httpClient = httpClient;
            _apiKey = configuration["OpenAI:ApiKey"];
            _logger = logger;
        }

        public async Task<string> GenerateSummaryAsync(string text)
        {
            int maxRetries = 3;
            int delayMs = 1000;

            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                try
                {
                    var requestBody = new
                    {
                        model = "gpt-3.5-turbo",
                        messages = new[]
                        {
                            new { role = "system", content = "You summarize news articles in 1-2 sentences." },
                            new { role = "user", content = text }
                        },
                        max_tokens = 100
                    };

                    var requestJson = JsonSerializer.Serialize(requestBody);
                    var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

                    var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", requestContent);

                    if ((int)response.StatusCode == 429) // rate limit
                    {
                        _logger?.LogWarning("Rate limit hit, retrying attempt {Attempt}", attempt);
                        await Task.Delay(delayMs * attempt);
                        continue;
                    }

                    response.EnsureSuccessStatusCode();

                    var responseContent = await response.Content.ReadAsStringAsync();
                    using var doc = JsonDocument.Parse(responseContent);
                    var summary = doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();

                    return summary ?? string.Empty;
                }
                catch (HttpRequestException ex)
                {
                    _logger?.LogWarning(ex, "HTTP error on attempt {Attempt}, retrying...", attempt);
                    await Task.Delay(delayMs * attempt);
                }
            }

            _logger?.LogError("Failed to generate summary after {MaxRetries} attempts", maxRetries);
            return string.Empty;
        }

    }
}
