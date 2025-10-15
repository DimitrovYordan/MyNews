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
        private readonly ChatClient _chatClient;
        private readonly ILogger<ChatGptService> _logger;
        private readonly string[] _targetLanguages;
        private const int BatchSize = 3;

        public ChatGptService(ChatClient chatClient, IOptions<LocalizationOptions> localizationOptions, ILogger<ChatGptService> logger)
        {
            _chatClient = chatClient;
            _logger = logger;
            _targetLanguages = localizationOptions.Value.TargetLanguages;
        }

        public async Task<List<EnrichedNewsDto>> EnrichBatchAsync(List<string> titles, CancellationToken cancellationToken = default)
        {
            var results = new List<EnrichedNewsDto>();
            if (titles == null || titles.Count == 0)
                return results;

            var chunks = SplitList(titles, BatchSize);

            foreach (var chunk in chunks)
            {
                var sectionTypes = string.Join(", ", Enum.GetNames(typeof(SectionType)));
                var systemPrompt =
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
                     $"Order must match input titles. Do NOT include extra commentary, explanations, code blocks or code fences (no ```).";

                var userContent = string.Join("\n", chunk.Select((t, idx) => $"{idx + 1}. {t}"));

                var messages = new List<ChatMessage>
                {
                    new SystemChatMessage(systemPrompt),
                    new UserChatMessage(userContent)
                };

                try
                {
                    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                    cts.CancelAfter(TimeSpan.FromMinutes(3));

                    var completion = await _chatClient.CompleteChatAsync(messages, cancellationToken: cts.Token);

                    var rawContent = completion.Value.Content.FirstOrDefault()?.Text ?? string.Empty;

                    var match = Regex.Match(rawContent, @"\[[\s\S]*\]");
                    if (!match.Success)
                    {
                        _logger.LogWarning("ChatGPT response did not contain JSON array. Raw response: {Raw}", rawContent);
                        results.AddRange(CreateFallback(chunk));
                        continue;
                    }

                    var json = JsonDocument.Parse(match.Value);
                    var arr = json.RootElement;

                    var parsedForChunk = new List<EnrichedNewsDto>();

                    foreach (var item in arr.EnumerateArray())
                    {
                        parsedForChunk.Add(ParseEnrichedFromJsonElement(item));
                    }

                    if (parsedForChunk.Count < chunk.Count)
                    {
                        for (int i = parsedForChunk.Count; i < chunk.Count; i++)
                            parsedForChunk.Add(CreateFallbackSingle(chunk[i]));
                    }

                    results.AddRange(parsedForChunk);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to enrich chunk. Using fallback for chunk.");
                    results.AddRange(CreateFallback(chunk));
                }
            }

            return results;
        }

        private EnrichedNewsDto ParseEnrichedFromJsonElement(JsonElement item)
        {
            string title = item.TryGetProperty("Title", out var pTitle) ? pTitle.GetString() ?? string.Empty : string.Empty;
            string summary = item.TryGetProperty("Summary", out var pSummary) ? pSummary.GetString() ?? string.Empty : string.Empty;
            string sectionStr = item.TryGetProperty("Section", out var pSection) ? pSection.GetString() ?? string.Empty : string.Empty;

            if (!Enum.TryParse(sectionStr?.Trim(), out SectionType section))
                section = SectionType.General;

            var translations = new Dictionary<string, NewsTranslationDto>();
            if (item.TryGetProperty("Translations", out var translationsJson) && translationsJson.ValueKind == JsonValueKind.Object)
            {
                foreach (var prop in translationsJson.EnumerateObject())
                {
                    var lang = prop.Name;
                    var tTitle = prop.Value.TryGetProperty("Title", out var tt) ? tt.GetString() ?? string.Empty : string.Empty;
                    var tSummary = prop.Value.TryGetProperty("Summary", out var ts) ? ts.GetString() ?? string.Empty : string.Empty;

                    translations[lang] = new NewsTranslationDto
                    {
                        LanguageCode = lang,
                        Title = tTitle,
                        Summary = tSummary
                    };
                }
            }
            else
            {
                foreach (var l in _targetLanguages)
                    translations[l] = new NewsTranslationDto { LanguageCode = l, Title = string.Empty, Summary = string.Empty };
            }

            return new EnrichedNewsDto
            {
                Title = title,
                Summary = summary,
                Section = section,
                Translations = translations
            };
        }

        private EnrichedNewsDto CreateFallbackSingle(string title)
        {
            var translations = new Dictionary<string, NewsTranslationDto>();
            foreach (var l in _targetLanguages)
                translations[l] = new NewsTranslationDto { LanguageCode = l, Title = string.Empty, Summary = string.Empty };

            return new EnrichedNewsDto
            {
                Title = title,
                Summary = "Summary unavailable",
                Section = SectionType.General,
                Translations = translations
            };
        }

        private List<EnrichedNewsDto> CreateFallback(List<string> titles)
        {
            return titles.Select(t => CreateFallbackSingle(t)).ToList();
        }

        private static List<List<T>> SplitList<T>(List<T> items, int size)
        {
            var result = new List<List<T>>();
            for (int i = 0; i < items.Count; i += size)
            {
                result.Add(items.GetRange(i, Math.Min(size, items.Count - i)));
            }
            return result;
        }
    }
}
