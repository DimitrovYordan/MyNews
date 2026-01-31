using Microsoft.Extensions.Options;

using System.Text.Json;

using MyNews.Api.DTOs;
using MyNews.Api.Enums;
using MyNews.Api.Interfaces;
using MyNews.Api.Options;

using OpenAI.Chat;
using FastText.NetWrapper;

namespace MyNews.Api.Services
{
    public class ChatGptService : IChatGptService
    {
        private readonly ChatClient _chatClient;
        private readonly NllbTranslationClient _nllbClient;
        private readonly ILogger<ChatGptService> _logger;
        private readonly string[] _targetLanguages;
        private readonly OpenAIOptions _options;

        public ChatGptService(ChatClient chatClient, NllbTranslationClient nllbClient, IOptions<LocalizationOptions> localizationOptions, IConfiguration configuration, ILogger<ChatGptService> logger, IOptions<OpenAIOptions> options)
        {
            _chatClient = chatClient;
            _nllbClient = nllbClient;
            _logger = logger;
            _targetLanguages = localizationOptions.Value.TargetLanguages;
            _options = options.Value;
        }

        public async Task<List<EnrichedNewsDto>> EnrichBatchAsync(List<NewsForEnrichmentDto> items, CancellationToken cancellationToken = default, List<string>? overrideLanguages = null)
        {
            var results = new List<EnrichedNewsDto>();
            if (items == null || items.Count == 0)
                return results;

            var detectLang = DetectLanguage(items[0].Title);

            var targetLanguages = (overrideLanguages ?? _targetLanguages.ToList()).Select(x => x.ToLowerInvariant()).ToList();

            var chunks = SplitList(items, _options.ChunkSize);

            foreach (var chunk in chunks)
            {
                var sectionTypes = string.Join(", ", Enum.GetNames(typeof(SectionType)));
                var systemPrompt =
                    $@"You are a strict JSON-only news enrichment assistant.

                        Return ONLY a JSON array of objects (no commentary, no markdown, no code fences).
                        The array must keep the same order as the input.
                        
                        Each output object must contain EXACTLY these keys:
                        - ""Title""              (original title, unchanged)
                        - ""Summary""            (summary in the original language, concise and informative in 2 to 4 sentences.)
                        - ""Section""            (one of: {sectionTypes})

                        FORMAT RULES:
                        - NEVER wrap the JSON in code fences.
                        - NEVER add comments.
                        - NEVER output anything except the JSON array.";

                var userLines = chunk.Select((c, idx) =>
                    $"{idx + 1}. Title: {EscapeForPrompt(c.Title)}\nContent: {EscapeForPrompt(c.ContentSnippet)}"
                );

                var messages = new List<ChatMessage>
                {
                    new SystemChatMessage(systemPrompt),
                    new UserChatMessage(string.Join("\n\n", userLines))
                };

                int attempts = 0;
                const int maxAttempts = 3;
                TimeSpan delay = TimeSpan.FromSeconds(1);
                bool success = false;

                while (attempts < maxAttempts && !success)
                {
                    attempts++;
                    try
                    {
                        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                        cts.CancelAfter(TimeSpan.FromMinutes(_options.TimeoutSeconds));

                        var completion = await _chatClient.CompleteChatAsync(messages, cancellationToken: cts.Token);
                        var raw = completion.Value.Content.FirstOrDefault()?.Text ?? string.Empty;
                        _logger.LogInformation("[AI RESPONSE RAW] {Raw}", raw);
                        var cleanedJson = raw.Trim().TrimStart('`').TrimEnd('`').Trim().TrimStart().Trim();
                        var json = JsonDocument.Parse(cleanedJson);
                        foreach (var item in json.RootElement.EnumerateArray())
                        {
                            results.Add(ParseEnrichedFromJsonElement(item));
                        }

                        if (results.Count < items.Count)
                        {
                            while (results.Count % chunk.Count != 0)
                            {
                                var idx = results.Count % chunk.Count;
                                results.Add(CreateFallbackSingle(chunk[idx].Title));
                            }
                        }

                        success = true;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "GPT attempt {Attempt} failed for chunk", attempts);
                        if (attempts >= maxAttempts)
                        {
                            _logger.LogError(ex, "GPT failed for chunk after {Attempts} attempts. Using fallback for chunk.", attempts);
                            results.AddRange(CreateFallback(chunk.Select(c => c.Title).ToList()));
                        }
                        else
                        {
                            await Task.Delay(delay, cancellationToken);
                            delay = delay * 2;
                        }
                    }
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

            return new EnrichedNewsDto
            {
                Title = title,
                Summary = summary,
                Section = section,
                SourceLanguage = DetectLanguage(title)
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
                Section = SectionType.General
            };
        }

        private static List<List<T>> SplitList<T>(List<T> items, int size)
        {
            var result = new List<List<T>>();
            for (int i = 0; i < items.Count; i += size)
                result.Add(items.GetRange(i, Math.Min(size, items.Count - i)));

            return result;
        }

        private static string EscapeForPrompt(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return string.Empty;

            return s.Replace("\r", " ").Replace("\n", " ").Trim();
        }

        private List<EnrichedNewsDto> CreateFallback(List<string> titles)
        {
            if (titles == null || titles.Count == 0)
                return new List<EnrichedNewsDto>();

            return titles.Select(t => CreateFallbackSingle(t)).ToList();
        }

        private string DetectLanguage(string text)
        {
            var path = AppContext.BaseDirectory;
            var modelPath = Path.Combine(AppContext.BaseDirectory, "DetectLanguage", "lid.176.bin");
            if (!File.Exists(modelPath))
            {
                _logger.LogError("FastText model not found at {Path}!", modelPath);
                
                return text.Any(c => (c >= 'а' && c <= 'я') || (c >= 'А' && c <= 'Я')) ? "bg" : "en";
            }

            using (var fastText = new FastTextWrapper())
            {
                fastText.LoadModel(modelPath);

                var predictions = fastText.PredictMultiple(text, 1);

                if (predictions.Any())
                {
                    var label = predictions.First().Label;

                    return label.Replace("__label__", "");
                }
            }

            return "other";
        }

        public async Task<Dictionary<string, NewsTranslationDto>> TranslateWithNllbAsync(string title, string summary, string sourceLang, List<string> targetLangs, CancellationToken cancellationToken)
        {
            return await _nllbClient.TranslateAsync(title, summary, sourceLang, targetLangs, cancellationToken);
        }
    }
}
