using Microsoft.Extensions.Options;

using System.Text.Json;
using System.Text.RegularExpressions;

using MyNews.Api.DTOs;
using MyNews.Api.Enums;
using MyNews.Api.Interfaces;
using MyNews.Api.Options;

using OpenAI.Chat;

namespace MyNews.Api.Services
{
    public class ChatGptService : IChatGptService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _model;
        private readonly ILogger<ChatGptService> _logger;
        private readonly int _batchChunkSize = 5;
        private readonly string[] _targetLanguages;

        public ChatGptService(HttpClient httpClient, IConfiguration configuration, IOptions<LocalizationOptions> localizationOptions, ILogger<ChatGptService> logger)
        {
            _httpClient = httpClient;
            _apiKey = configuration["OpenAI:ApiKey"];
            _model = configuration["OpenAI:Model"] ?? "gpt-4o-mini";
            _logger = logger;
            _targetLanguages = localizationOptions.Value.TargetLanguages;
        }

        public async Task<List<EnrichedNewsDto>> EnrichNewsBatchAsync(List<string> titles, CancellationToken cancellationToken = default)
        {
            var results = new List<EnrichedNewsDto>();

            for (int i = 0; i < titles.Count; i += _batchChunkSize)
            {
                var chunk = titles.Skip(i).Take(_batchChunkSize).ToList();
                var sectionTypes = string.Join(", ", Enum.GetNames(typeof(SectionType)));

                var messages = new List<ChatMessage>
                {
                    new SystemChatMessage(
                        $"You are a news enrichment assistant.\n\n" +
                        $"For each input news title:\n" +
                        $"1. Write a concise and informative Summary in 2 to 4 sentences.\n" +
                        $"- Clearly explain the main point.\n" +
                        $"- Avoid unnecessary details or filler.\n" +
                        $"- No speculation.\n" +
                        $"2. Assign a Section from: {sectionTypes}\n" +
                        $"3. Provide Translations ONLY for the following target languages: {string.Join(", ", _targetLanguages)}\n" +
                        $"- Each translation must be an object with keys 'Title' and 'Summary'\n" +
                        $"- Do NOT include any other languages\n\n" +
                        $"Return a valid JSON array containing exactly 'Title', 'Summary', 'Section', and 'Translations'.\n" +
                        $"Order must match input titles. Do NOT include extra commentary, explanations, code blocks or code fences (no ```)."
                    ),
                    new UserChatMessage(string.Join("\n", chunk.Select((t, idx) => $"{idx + 1}. {t}")))
                };

                try
                {
                    var chatClient = new ChatClient(
                        model: _model,
                        apiKey: _apiKey
                    );

                    var completion = await chatClient.CompleteChatAsync(messages);

                    var rawContent = completion.Value.Content.FirstOrDefault()?.Text ?? string.Empty;
                    var match = Regex.Match(rawContent, @"\[[\s\S]*\]");
                    if (!match.Success)
                    {
                        results.AddRange(CreateFallback(chunk));
                        continue;
                    }

                    var contentStr = match.Value;
                    var json = JsonDocument.Parse(contentStr);
                    foreach (var item in json.RootElement.EnumerateArray())
                    {
                        var title = item.GetProperty("Title").GetString() ?? string.Empty;
                        var summary = item.GetProperty("Summary").GetString() ?? string.Empty;
                        var sectionStr = item.GetProperty("Section").GetString() ?? string.Empty;

                        if (!Enum.TryParse(sectionStr.Trim(), out SectionType section))
                            section = SectionType.General;

                        var translations = new Dictionary<string, (string Title, string Summary)>();
                        if (item.TryGetProperty("Translations", out var translationsJson))
                        {
                            foreach (var prop in translationsJson.EnumerateObject())
                            {
                                var tTitle = prop.Value.GetProperty("Title").GetString() ?? string.Empty;
                                var tSummary = prop.Value.GetProperty("Summary").GetString() ?? string.Empty;
                                translations[prop.Name] = (tTitle, tSummary);
                            }
                        }

                        results.Add(new EnrichedNewsDto
                        {
                            Title = title,
                            Summary = summary,
                            Section = section,
                            Translations = translations
                        });
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to process GPT batch. Using fallback for entire chunk.");
                    results.AddRange(CreateFallback(chunk));
                }
            }

            return results;
        }

        private List<EnrichedNewsDto> CreateFallback(List<string> chunk)
        {
            return chunk.Select(t => new EnrichedNewsDto
            {
                Title = t,
                Summary = "Summary unavailable",
                Section = SectionType.General,
                Translations = _targetLanguages.ToDictionary(l => l, l => (Title: string.Empty, Summary: string.Empty))
            }).ToList();
        }
    }
}
