using Microsoft.Extensions.Options;

using System.Text.Json;
using System.Text.RegularExpressions;

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
        private readonly ILogger<ChatGptService> _logger;
        private readonly string[] _targetLanguages;
        private readonly OpenAIOptions _options;

        public ChatGptService(ChatClient chatClient, IOptions<LocalizationOptions> localizationOptions, IConfiguration configuration, ILogger<ChatGptService> logger, IOptions<OpenAIOptions> options)
        {
            _chatClient = chatClient;
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
                        - ""Translations""       (object with translated versions)

                        LANGUAGE DETECTION RULE:
                        - ORIGINAL_LANGUAGE = ""{detectLang}""
                        
                        LANGUAGE RULES:
                        - If ORIGINAL_LANGUAGE = ""bg"": include ONLY ""en""
                        - If ORIGINAL_LANGUAGE = ""en"": include ONLY ""bg""
                        - If ORIGINAL_LANGUAGE = ""other"": 
                            YOU MUST ALWAYS provide real translations for both ""en"" and ""bg"".
                            Never return empty strings for these languages unless the text is literally non-translateable.
                        - NEVER include a key matching the original language.

                        TRANSLATION OBJECT RULES:
                        - ""Translations"" must ALWAYS contain all required target languages.
                        - Each target language must contain:
                            {{ ""Title"": ""..."", ""Summary"": ""..."" }}
                        
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
                            results.Add(ParseEnrichedFromJsonElement(item, targetLanguages));
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

        public async Task<Dictionary<string, NewsTranslationDto>> TranslateTitlesAndSummariesAsync(string title, string summary, List<string> targetLangs, CancellationToken cancellationToken)
        {
            var translations = new Dictionary<string, NewsTranslationDto>();
            if (targetLangs == null || targetLangs.Count == 0)
                return translations;

            string prompt = $"Translate the following news title and summary into the specified languages: {string.Join(", ", targetLangs)}.\n" +
                            $"Return valid JSON with one object per language, keys: 'Title', 'Summary'.\n" +
                            $"No commentary or markdown.\n\n" +
                            $"Title: {title}\nSummary: {summary}";

            var messages = new List<ChatMessage>
            {
                new SystemChatMessage("You are a multilingual translation assistant."),
                new UserChatMessage(prompt)
            };

            try
            {
                var completion = await _chatClient.CompleteChatAsync(messages, cancellationToken: cancellationToken);
                var raw = completion.Value.Content.FirstOrDefault()?.Text ?? string.Empty;
                _logger.LogInformation("[TranslateTitlesAndSummariesAsync - AI RESPONSE RAW] {Raw}", raw);
                var match = Regex.Match(raw, @"\{[\s\S]*\}");
                if (!match.Success) 
                    return translations;

                var json = JsonDocument.Parse(match.Value);
                foreach (var lang in json.RootElement.EnumerateObject())
                {
                    var titleTr = lang.Value.TryGetProperty("Title", out var tt) ? tt.GetString() ?? string.Empty : string.Empty;
                    var sumTr = lang.Value.TryGetProperty("Summary", out var ss) ? ss.GetString() ?? string.Empty : string.Empty;

                    translations[lang.Name.ToUpper()] = new NewsTranslationDto
                    {
                        LanguageCode = lang.Name.ToUpper(),
                        Title = titleTr,
                        Summary = sumTr
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Translation GPT call failed for {Title}", title);
            }

            return translations;
        }

        private EnrichedNewsDto ParseEnrichedFromJsonElement(JsonElement item, List<string> targetLanguages)
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
                    if (prop.Value.ValueKind == JsonValueKind.Object)
                    {
                        var tTitle = prop.Value.TryGetProperty("Title", out var tt) ? tt.GetString() ?? string.Empty : string.Empty;
                        var tSummary = prop.Value.TryGetProperty("Summary", out var ts) ? ts.GetString() ?? string.Empty : string.Empty;

                        translations[lang] = new NewsTranslationDto
                        {
                            LanguageCode = lang,
                            Title = tTitle,
                            Summary = tSummary
                        };
                    }
                    else
                    {
                        translations[lang] = new NewsTranslationDto
                        {
                            LanguageCode = lang,
                            Title = string.Empty,
                            Summary = string.Empty
                        };
                    }
                }
            }
            else
            {
                foreach (var l in targetLanguages)
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
            var modelPath = Path.Combine(path, "DetectLanguage", "lid.176.bin");
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
    }
}
