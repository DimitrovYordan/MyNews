using Microsoft.Extensions.Options;

using System.Diagnostics;
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

            _logger.LogInformation("Starting enrichment for {TotalTitles} titles (chunk size {ChunkSize})", titles.Count, _batchChunkSize);

            for (int i = 0; i < titles.Count; i += _batchChunkSize)
            {
                var chunk = titles.Skip(i).Take(_batchChunkSize).ToList();
                _logger.LogDebug("Processing chunk {ChunkIndex} (count {ChunkCount}): firstTitlesPreview: {Preview}",
                    i / _batchChunkSize + 1, chunk.Count, string.Join(" | ", chunk.Take(5)));

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

                Stopwatch gptStopwatch = Stopwatch.StartNew();
                try
                {
                    _logger.LogDebug("Sending request to ChatGPT (model: {Model}) for chunk {ChunkIndex}", _model, i / _batchChunkSize + 1);

                    var chatClient = new ChatClient(
                        model: _model,
                        apiKey: _apiKey
                    );

                    var completion = await chatClient.CompleteChatAsync(messages);

                    gptStopwatch.Stop();
                    _logger.LogDebug("Received GPT response in {Elapsed} ms for chunk {ChunkIndex}", gptStopwatch.ElapsedMilliseconds, i / _batchChunkSize + 1);

                    var rawContent = completion.Value.Content.FirstOrDefault()?.Text ?? string.Empty;
                    _logger.LogDebug("GPT raw content length: {Length}", rawContent?.Length ?? 0);

                    var match = Regex.Match(rawContent, @"\[[\s\S]*\]");
                    if (!match.Success)
                    {
                        _logger.LogWarning("GPT response did not contain a JSON array for chunk {ChunkIndex}. Raw Content (first 200 chars): {Preview}", i / _batchChunkSize + 1, rawContent?.Substring(0, Math.Min(200, rawContent.Length)) ?? "");
                        results.AddRange(CreateFallback(chunk));
                        continue;
                    }

                    var contentStr = match.Value;
                    try
                    {
                        var json = JsonDocument.Parse(contentStr);
                        int parsedCount = 0;
                        foreach (var item in json.RootElement.EnumerateArray())
                        {
                            parsedCount++;
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

                            _logger.LogDebug("Parsed enriched item #{Index} (chunk {ChunkIndex}): Title='{Title}', Section='{Section}', TranslationsCount={Count}", parsedCount, i / _batchChunkSize + 1, title, section, translations.Count);

                            results.Add(new EnrichedNewsDto
                            {
                                Title = title,
                                Summary = summary,
                                Section = section,
                                Translations = translations
                            });
                        }

                        _logger.LogInformation("Chunk {ChunkIndex} parsed successfully: {ParsedCount} items.", i / _batchChunkSize + 1, parsedCount);
                    }
                    catch (Exception exParse)
                    {
                        _logger.LogError(exParse, "Failed to parse GPT JSON for chunk {ChunkIndex}. Content preview: {Preview}", i / _batchChunkSize + 1, contentStr?.Substring(0, Math.Min(500, contentStr.Length)) ?? "");
                        results.AddRange(CreateFallback(chunk));
                    }
                }
                catch (Exception ex)
                {
                    gptStopwatch.Stop();
                    _logger.LogError(ex, "ChatGPT request failed for chunk {ChunkIndex}. Falling back for this chunk.", i / _batchChunkSize + 1);
                    _logger.LogDebug("Chunk content preview: {Preview}", string.Join(" | ", chunk.Take(10)));
                    results.AddRange(CreateFallback(chunk));
                }
            }

            _logger.LogInformation("Enrichment finished. Total results: {Count}", results.Count);
            return results;
        }

        private List<EnrichedNewsDto> CreateFallback(List<string> chunk)
        {
            _logger.LogDebug("Creating fallback enriched items for chunk size {ChunkSize}", chunk.Count);

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
