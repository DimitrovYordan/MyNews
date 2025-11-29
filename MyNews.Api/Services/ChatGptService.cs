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
        private readonly OpenAIOptions _options;

        public ChatGptService(ChatClient chatClient, IOptions<LocalizationOptions> localizationOptions,
            IConfiguration configuration, ILogger<ChatGptService> logger, IOptions<OpenAIOptions> options)
        {
            _chatClient = chatClient;
            _logger = logger;
            _targetLanguages = localizationOptions.Value.TargetLanguages ?? Array.Empty<string>();
            _options = options.Value;
        }

        public async Task<List<EnrichedNewsDto>> EnrichBatchAsync(List<NewsForEnrichmentDto> items, CancellationToken cancellationToken = default, List<string>? overrideLanguages = null)
        {
            var results = new List<EnrichedNewsDto>();
            if (items == null || items.Count == 0)
                return results;

            var targetLanguages = (overrideLanguages ?? _targetLanguages.ToList())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.ToLowerInvariant())
                .ToList();

            var chunks = SplitList(items, _options.ChunkSize);

            foreach (var chunk in chunks)
            {
                var sectionTypes = string.Join(", ", Enum.GetNames(typeof(SectionType)));
                var stablePrompt =
                    $@"You are a strict JSON-only news enrichment assistant.
                    Return ONLY a JSON array of objects (no commentary, no markdown, no code fences).
                    The array must have the same item order as the input.
                    Each object must contain exactly these keys: ""Title"", ""Summary"", ""Section"", ""Translations"".
                    
                    FIELD RULES:
                    - Section: one of: {sectionTypes}
                    - Translations: an object where keys are language codes (lowercase) and values are objects with keys:
                        - Title: translated title
                        - Summary: translated summary
                    
                    LANGUAGE RULES:
                    1. Detect the language of the input automatically.
                    2. The ""Title"" and ""Summary"" fields must ALWAYS return the original text (no translation).
                    3. If the input is in **Bulgarian**, return only:
                           ""en"" translation.
                    4. If the input is in **English**, return:
                           ""bg"" translation.
                    5. If the input is in ANY other language:
                           return **both** ""en"" and ""bg"" translations.
                    6. NEVER return a translation into the same language as the original.
                    7. If translation is impossible, return:
                           Title: """"
                           Summary: """"
                    
                    The JSON MUST NOT contain any unexpected keys.
                    
                    VALID OUTPUT STRUCTURE EXAMPLE (structure only, not content):
                    [
                      {{
                        ""Title"": ""Original title"",
                        ""Summary"": ""Original summary text."",
                        ""Section"": ""General"",
                        ""Translations"": {{
                            ""en"": {{ ""Title"": ""Translated title"", ""Summary"": ""Translated summary"" }}
                        }}
                      }}
                    ]";

                var userLines = chunk.Select((c, idx) =>
                    $"{idx + 1}. Title: {EscapeForPrompt(c.Title)}\nLink: {c.Link}\nContent: {EscapeForPrompt(c.ContentSnippet)}"
                );

                var messages = new List<ChatMessage>
                {
                    new SystemChatMessage(stablePrompt),
                    new UserChatMessage(string.Join("\n\n", userLines))
                };

                _logger.LogInformation("[GPT] Sending chunk (count={Count}) Titles={Titles}", chunk.Count, string.Join(" | ", chunk.Select(c => c.Title)));
                _logger.LogDebug("[GPT] Payload for chunk:\n{Payload}", string.Join("\n\n", userLines));

                int attempts = 0;
                const int maxAttempts = 3;
                TimeSpan delay = TimeSpan.FromSeconds(1);
                bool success = false;

                var parsedForChunk = new List<EnrichedNewsDto>();

                while (attempts < maxAttempts && !success && !cancellationToken.IsCancellationRequested)
                {
                    attempts++;
                    try
                    {
                        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                        cts.CancelAfter(TimeSpan.FromMinutes(3));

                        var completion = await _chatClient.CompleteChatAsync(messages, cancellationToken: cts.Token);
                        var raw = completion.Value.Content.FirstOrDefault()?.Text ?? string.Empty;

                        _logger.LogInformation("[GPT] Raw response (attempt {Attempt}): {TruncatedRaw}", attempts, TruncateForLog(raw, 2000));
                        _logger.LogDebug("[GPT] Full raw response (attempt {Attempt}):\n{Raw}", attempts, raw);

                        var match = Regex.Match(raw, @"\[\s*\{[\s\S]*\}\s*\]", RegexOptions.Multiline);
                        if (!match.Success)
                        {
                            _logger.LogWarning("[GPT] No JSON array-of-objects found in response (attempt {Attempt}). Raw starts: {Start}", attempts, TruncateForLog(raw, 200));
                            throw new InvalidOperationException("GPT response missing valid JSON array-of-objects.");
                        }

                        var extracted = match.Value;
                        _logger.LogDebug("[GPT] Extracted JSON for parsing (attempt {Attempt}): {JsonPreview}", attempts, TruncateForLog(extracted, 2000));

                        JsonDocument jsonDoc;
                        try
                        {
                            jsonDoc = JsonDocument.Parse(extracted);
                        }
                        catch (Exception parseEx)
                        {
                            _logger.LogError(parseEx, "[GPT] JSON parse failed (attempt {Attempt}). Extracted JSON: {Extracted}", attempts, TruncateForLog(extracted, 2000));
                            throw;
                        }

                        var root = jsonDoc.RootElement;
                        if (root.ValueKind != JsonValueKind.Array)
                        {
                            _logger.LogWarning("[GPT] Extracted JSON root is not array (attempt {Attempt}). RootKind={Kind}", attempts, root.ValueKind);
                            throw new InvalidOperationException("Extracted JSON is not an array.");
                        }

                        int jsonIndex = 0;
                        foreach (var item in root.EnumerateArray())
                        {
                            if (jsonIndex >= chunk.Count)
                            {
                                _logger.LogWarning("[GPT] JSON returned more items than input chunk. Truncating extras.");
                                break;
                            }

                            if (item.ValueKind != JsonValueKind.Object)
                            {
                                _logger.LogWarning("[GPT] JSON element is not an object (index={Index}). ValueKind={Kind}. Using fallback for this position.", jsonIndex, item.ValueKind);
                                parsedForChunk.Add(CreateFallbackSingle(chunk[jsonIndex].Title, targetLanguages));
                                jsonIndex++;
                                continue;
                            }

                            try
                            {
                                parsedForChunk.Add(ParseEnrichedFromJsonElement(item, targetLanguages));
                            }
                            catch (Exception exParseItem)
                            {
                                _logger.LogWarning(exParseItem, "[GPT] Failed to parse JSON element at index {Index}. Using fallback for this position. Raw element: {Elem}", jsonIndex, TruncateForLog(item.ToString(), 500));
                                parsedForChunk.Add(CreateFallbackSingle(chunk[jsonIndex].Title, targetLanguages));
                            }

                            jsonIndex++;
                        }

                        while (parsedForChunk.Count < chunk.Count)
                        {
                            var idx = parsedForChunk.Count;
                            _logger.LogWarning("[GPT] Missing item for chunk position {Idx}. Adding fallback for title: {Title}", idx, chunk[idx].Title);
                            parsedForChunk.Add(CreateFallbackSingle(chunk[idx].Title, targetLanguages));
                        }

                        results.AddRange(parsedForChunk);
                        success = true;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "[GPT] Attempt {Attempt} failed for chunk (titles: {Titles})", attempts, string.Join(" | ", chunk.Select(c => c.Title)));
                        if (attempts >= maxAttempts)
                        {
                            _logger.LogError(ex, "[GPT] Failed for chunk after {Attempts} attempts. Adding fallback for entire chunk.", attempts);
                            results.AddRange(CreateFallback(chunk.Select(c => c.Title).ToList(), targetLanguages));
                        }
                        else
                        {
                            _logger.LogInformation("[GPT] Will retry attempt {NextAttempt} after {Delay}ms", attempts + 1, (int)delay.TotalMilliseconds);
                            await Task.Delay(delay, cancellationToken);
                            delay = TimeSpan.FromMilliseconds(delay.TotalMilliseconds * 2 + new Random().Next(0, 200));
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
                new SystemChatMessage("You are a multilingual translation assistant. Return only JSON."),
                new UserChatMessage(prompt)
            };

            try
            {
                var completion = await _chatClient.CompleteChatAsync(messages, cancellationToken: cancellationToken);
                var raw = completion.Value.Content.FirstOrDefault()?.Text ?? string.Empty;
                _logger.LogInformation("[GPT-TRANSLATE] Raw response: {TruncatedRaw}", TruncateForLog(raw, 1000));
                _logger.LogDebug("[GPT-TRANSLATE] Full raw response:\n{Raw}", raw);

                var match = Regex.Match(raw, @"\{[\s\S]*\}");
                if (!match.Success)
                {
                    _logger.LogWarning("[GPT-TRANSLATE] No JSON object found in response for title {Title}. Raw starts: {Start}", title, TruncateForLog(raw, 200));
                    return translations;
                }

                var json = JsonDocument.Parse(match.Value);
                if (json.RootElement.ValueKind != JsonValueKind.Object)
                {
                    _logger.LogWarning("[GPT-TRANSLATE] Parsed root is not object for title {Title}. RootKind={Kind}", title, json.RootElement.ValueKind);
                    return translations;
                }

                foreach (var lang in json.RootElement.EnumerateObject())
                {
                    if (lang.Value.ValueKind != JsonValueKind.Object)
                        continue;

                    var titleTr = lang.Value.TryGetProperty("Title", out var tt) && tt.ValueKind == JsonValueKind.String ? tt.GetString() ?? string.Empty : string.Empty;
                    var sumTr = lang.Value.TryGetProperty("Summary", out var ss) && ss.ValueKind == JsonValueKind.String ? ss.GetString() ?? string.Empty : string.Empty;

                    translations[lang.Name.ToUpperInvariant()] = new NewsTranslationDto
                    {
                        LanguageCode = lang.Name.ToUpperInvariant(),
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
            if (item.ValueKind != JsonValueKind.Object)
                throw new InvalidOperationException("Expected JSON object.");

            string title = string.Empty;
            if (item.TryGetProperty("Title", out var pTitle) && pTitle.ValueKind == JsonValueKind.String)
                title = pTitle.GetString() ?? string.Empty;

            string summary = string.Empty;
            if (item.TryGetProperty("Summary", out var pSummary) && pSummary.ValueKind == JsonValueKind.String)
                summary = pSummary.GetString() ?? string.Empty;

            string sectionStr = string.Empty;
            if (item.TryGetProperty("Section", out var pSection) && pSection.ValueKind == JsonValueKind.String)
                sectionStr = pSection.GetString() ?? string.Empty;

            if (!Enum.TryParse(sectionStr?.Trim(), out SectionType section))
                section = SectionType.General;

            var translations = new Dictionary<string, NewsTranslationDto>(StringComparer.OrdinalIgnoreCase);

            if (item.TryGetProperty("Translations", out var translationsJson) && translationsJson.ValueKind == JsonValueKind.Object)
            {
                foreach (var prop in translationsJson.EnumerateObject())
                {
                    var lang = prop.Name.ToLowerInvariant();
                    if (prop.Value.ValueKind != JsonValueKind.Object)
                    {
                        translations[lang] = new NewsTranslationDto { LanguageCode = lang, Title = string.Empty, Summary = string.Empty };
                        continue;
                    }

                    var tTitle = prop.Value.TryGetProperty("Title", out var tt) && tt.ValueKind == JsonValueKind.String ? tt.GetString() ?? string.Empty : string.Empty;
                    var tSummary = prop.Value.TryGetProperty("Summary", out var ts) && ts.ValueKind == JsonValueKind.String ? ts.GetString() ?? string.Empty : string.Empty;

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
                foreach (var l in targetLanguages)
                    translations[l.ToLowerInvariant()] = new NewsTranslationDto { LanguageCode = l.ToLowerInvariant(), Title = string.Empty, Summary = string.Empty };
            }

            return new EnrichedNewsDto
            {
                Title = string.IsNullOrWhiteSpace(title) ? "Title unavailable" : title,
                Summary = string.IsNullOrWhiteSpace(summary) ? "Summary unavailable" : summary,
                Section = section,
                Translations = translations
            };
        }

        private List<EnrichedNewsDto> CreateFallback(List<string> titles, List<string> targetLanguages)
        {
            if (titles == null || titles.Count == 0)
                return new List<EnrichedNewsDto>();

            return titles.Select(t => CreateFallbackSingle(t, targetLanguages)).ToList();
        }

        private EnrichedNewsDto CreateFallbackSingle(string title, List<string> targetLanguages)
        {
            var translations = new Dictionary<string, NewsTranslationDto>(StringComparer.OrdinalIgnoreCase);
            foreach (var l in targetLanguages.Distinct(StringComparer.OrdinalIgnoreCase))
                translations[l.ToLowerInvariant()] = new NewsTranslationDto { LanguageCode = l.ToLowerInvariant(), Title = string.Empty, Summary = string.Empty };

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

            var cleaned = s.Replace("\r", " ").Replace("\n", " ").Trim();
            if (cleaned.Length > 2000)
                return cleaned.Substring(0, 2000);

            return cleaned;
        }

        private static string TruncateForLog(string s, int max)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;
            if (s.Length <= max) return s;
            return s.Substring(0, max) + "...(truncated)";
        }
    }
}