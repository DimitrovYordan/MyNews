using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

using MyNews.Api.Enums;
using MyNews.Api.Interfaces;

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

        public async Task<(string Summary, SectionType Section)> EnrichNewsAsync(string title)
        {
            try
            {
                var sectionTypes = string.Join(", ", Enum.GetNames(typeof(SectionType)));
                var systemMessage = $"You summarize news articles in 1-2 sentences and assign them to a section from the following options: {sectionTypes}. Return a valid JSON object like {{ \"Summary\": \"...\", \"Section\": \"...\" }}.";

                var requestBody = new
                {
                    model = "gpt-3.5-turbo",
                    messages = new[]
                    {
                        new { role = "system", content = systemMessage },
                        new { role = "user", content = $"News title: \"{title}\"" }
                    },
                    max_tokens = 150
                };

                var requestJson = JsonSerializer.Serialize(requestBody);
                var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
                var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", requestContent);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(responseContent);
                var contentStr = doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();

                if (!string.IsNullOrWhiteSpace(contentStr))
                {
                    try
                    {
                        var json = JsonDocument.Parse(contentStr);
                        string summary = json.RootElement.GetProperty("Summary").GetString() ?? string.Empty;
                        string sectionStr = json.RootElement.GetProperty("Section").GetString() ?? string.Empty;

                        if (Enum.TryParse<SectionType>(sectionStr.Trim(), out var section))
                            return (summary, section);
                        else
                        {
                            _logger.LogWarning("GPT returned unknown section '{SectionStr}' for title: {Title}", sectionStr, title);
                            return (summary, SectionType.General);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to parse GPT JSON for title: {Title}", title);
                    }
                }
                else
                {
                    _logger.LogWarning("GPT returned empty content for title: {Title}", title);
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP error calling OpenAI API for title: {Title}", title);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in EnrichNewsAsync for title: {Title}", title);
            }

            return ("Summary unavailable", SectionType.General);
        }
    }
}
