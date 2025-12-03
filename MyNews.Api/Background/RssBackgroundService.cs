using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using System.Text.RegularExpressions;

using MyNews.Api.Data;
using MyNews.Api.DTOs;
using MyNews.Api.Interfaces;
using MyNews.Api.Models;
using MyNews.Api.Options;

using HtmlAgilityPack;

namespace MyNews.Api.Background
{
    public class RssBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<RssBackgroundService> _logger;
        private readonly int _rssFetchIntervalHours;
        private readonly OpenAIOptions _aiOptions;
        private readonly int _fetchArticleTimeoutSeconds = 10;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string[] _globalTargetLanguages;

        public RssBackgroundService(IServiceScopeFactory scopeFactory, ILogger<RssBackgroundService> logger, IOptions<BackgroundJobsOptions> bjOptions, 
            IHttpClientFactory httpClientFactory, IOptions<LocalizationOptions> localizationOptions, IOptions<OpenAIOptions> aiOptions)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _rssFetchIntervalHours = bjOptions.Value.RssFetchIntervalHours;
            _httpClientFactory = httpClientFactory;
            _globalTargetLanguages = localizationOptions.Value.TargetLanguages
                .Select(s => s?.Trim().ToLowerInvariant() ?? string.Empty)
                .Where(s => !string.IsNullOrEmpty(s))
                .ToArray();
            _aiOptions = aiOptions.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("RSS background service started at {Time}", DateTime.UtcNow);

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    var rssService = scope.ServiceProvider.GetRequiredService<IRssService>();

                    int totalNewsAdded = 0;
                    int totalSourcesProcessed = 0;
                    var batchStart = DateTime.UtcNow;

                    var sources = await dbContext.Sources.Skip(15).ToListAsync(cancellationToken);
                    int totalSources = sources.Count;
                    int srcIndex = 1;

                    foreach (var source in sources)
                    {
                        totalSourcesProcessed++;
                        _logger.LogInformation("[RSS] Processing source {Index}/{Total}: {Url}", srcIndex, totalSources, source.Url);

                        try
                        {
                            var rssItems = await rssService.FetchAndProcessRssFeedAsync(new[] { source });

                            var freshItems = new List<NewsItemDto>();
                            foreach (var rssItem in rssItems)
                            {
                                if (rssItem.PublishedAt < DateTime.UtcNow.AddDays(-2))
                                    continue;

                                bool existsInDb = await dbContext.NewsItems
                                    .AnyAsync(n => n.SourceId == source.Id &&
                                                  (n.Title == rssItem.Title || n.Link == rssItem.Link), cancellationToken);

                                bool existsInBatch = freshItems.Any(n => n.Link == rssItem.Link && n.SourceUrl == source.Url);

                                if (!existsInDb && !existsInBatch)
                                    freshItems.Add(rssItem);
                            }

                            if (!freshItems.Any())
                            {
                                srcIndex++;
                                continue;
                            }

                            var batches = SplitList(freshItems, _aiOptions.BatchSize);
                            using var semaphore = new SemaphoreSlim(_aiOptions.Concurrency);
                            var batchTasks = new List<Task>();

                            foreach (var batch in batches)
                            {
                                await semaphore.WaitAsync(cancellationToken);

                                var task = Task.Run(async () =>
                                {
                                    try
                                    {
                                        using var taskScope = _scopeFactory.CreateScope();
                                        var taskChat = taskScope.ServiceProvider.GetRequiredService<IChatGptService>();
                                        var taskDb = taskScope.ServiceProvider.GetRequiredService<AppDbContext>();

                                        var inputs = new List<NewsForEnrichmentDto>();
                                        foreach (var rssItem in batch)
                                        {
                                            string snippet = string.Empty;
                                            if (!string.IsNullOrWhiteSpace(rssItem.Description) && IsValidDescription(rssItem.Description))
                                            {
                                                snippet = rssItem.Description!;
                                            }
                                            else
                                            {
                                                try
                                                {
                                                    snippet = await FetchAndExtractArticleAsync(rssItem.Link, cancellationToken);
                                                }
                                                catch (Exception exFetch)
                                                {
                                                    _logger.LogWarning(exFetch, "Failed to fetch article for {Link}", rssItem.Link);
                                                    snippet = string.Empty;
                                                }
                                            }

                                            if (string.IsNullOrWhiteSpace(snippet))
                                            {
                                                snippet = rssItem.Title;
                                            }

                                            snippet = TruncateToSentenceBoundary(snippet, _aiOptions.MaxContentChars);
                                            inputs.Add(new NewsForEnrichmentDto
                                            {
                                                Title = rssItem.Title,
                                                ContentSnippet = snippet,
                                                Link = rssItem.Link
                                            });
                                        }

                                        List<string> targetLanguages = _globalTargetLanguages.ToList();

                                        var enrichedResults = await taskChat.EnrichBatchAsync(inputs, cancellationToken, targetLanguages);
                                        var needTranslation = enrichedResults
                                            .Where(e => e.Summary != "Summary unavailable" &&
                                                        e.Translations.Values.Any(t => string.IsNullOrWhiteSpace(t.Summary)))
                                            .ToList();

                                        if (needTranslation.Any())
                                        {
                                            _logger.LogInformation("[GPT] Running secondary translation pass for {Count} items", needTranslation.Count);

                                            foreach (var enriched in needTranslation)
                                            {
                                                var missingLangs = enriched.Translations
                                                    .Where(t => string.IsNullOrWhiteSpace(t.Value.Summary))
                                                    .Select(t => t.Key.ToUpper())
                                                    .ToList();

                                                if (!missingLangs.Any())
                                                    continue;

                                                try
                                                {
                                                    var translations = await taskChat.TranslateTitlesAndSummariesAsync(
                                                        enriched.Title,
                                                        enriched.Summary == "Summary unavailable" ? enriched.Title : enriched.Summary,
                                                        missingLangs,
                                                        cancellationToken);

                                                    foreach (var lang in translations.Keys)
                                                    {
                                                        enriched.Translations[lang] = new NewsTranslationDto
                                                        {
                                                            LanguageCode = lang,
                                                            Title = translations[lang].Title ?? enriched.Title,
                                                            Summary = translations[lang].Summary ?? enriched.Summary
                                                        };
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    _logger.LogWarning(ex, "Translation retry failed for {Title}", enriched.Title);
                                                }
                                            }
                                        }

                                        foreach (var (enriched, rssItem) in enrichedResults.Zip(batch))
                                        {
                                            var existingNews = await taskDb.NewsItems
                                                .Include(n => n.Translations)
                                                .FirstOrDefaultAsync(n =>
                                                    n.SourceId == source.Id &&
                                                    (n.Link == rssItem.Link || n.Title == rssItem.Title),
                                                    cancellationToken);

                                            NewsItem newsItem;
                                            if (existingNews != null)
                                            {
                                                newsItem = existingNews;
                                            }
                                            else
                                            {
                                                newsItem = new NewsItem
                                                {
                                                    Id = Guid.NewGuid(),
                                                    Section = enriched.Section,
                                                    Title = enriched.Title,
                                                    Summary = enriched.Summary,
                                                    Link = rssItem.Link,
                                                    PublishedAt = rssItem.PublishedAt,
                                                    FetchedAt = DateTime.UtcNow,
                                                    SourceId = source.Id
                                                };

                                                taskDb.NewsItems.Add(newsItem);
                                                Interlocked.Increment(ref totalNewsAdded);
                                            }

                                            foreach (var kv in enriched.Translations)
                                            {
                                                var lang = kv.Key.ToUpperInvariant();
                                                var tDto = kv.Value ?? new NewsTranslationDto { LanguageCode = lang, Title = enriched.Title, Summary = enriched.Summary };

                                                var existingTranslation = newsItem.Translations?
                                                    .FirstOrDefault(t => t.LanguageCode == lang);
                                                if (existingTranslation != null)
                                                {
                                                    existingTranslation.Title = tDto.Title ?? existingTranslation.Title;
                                                    existingTranslation.Summary = tDto.Summary ?? existingTranslation.Summary;
                                                }
                                                else
                                                {
                                                    var newTranslation = new NewsTranslation
                                                    {
                                                        Id = Guid.NewGuid(),
                                                        NewsItemId = newsItem.Id,
                                                        LanguageCode = lang,
                                                        Title = tDto.Title ?? string.Empty,
                                                        Summary = tDto.Summary ?? string.Empty,
                                                        Section = enriched.Section
                                                    };

                                                    taskDb.NewsTranslations.Add(newTranslation);
                                                }
                                            }

                                            await SaveWithRetryAsync(taskDb, enriched.Title, cancellationToken);
                                        }

                                        _logger.LogInformation("[RSS] Batch saved ({Count}) for source {Url}", enrichedResults.Count, source.Url);
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.LogError(ex, "[RSS] Error while processing batch for {Url}", source.Url);
                                    }
                                    finally
                                    {
                                        semaphore.Release();
                                    }
                                }, cancellationToken);

                                batchTasks.Add(task);
                            }

                            await Task.WhenAll(batchTasks);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error while processing source {Url}", source.Url);
                        }

                        srcIndex++;
                    }

                    var batchEnd = DateTime.UtcNow;
                    _logger.LogInformation("RSS batch finished at {Time}. Sources processed: {SourcesCount}. Total news added: {NewsCount}. Duration: {Duration}s",
                        batchEnd, totalSourcesProcessed, totalNewsAdded, (batchEnd - batchStart).TotalSeconds);
                }
                catch (Exception exOuter)
                {
                    _logger.LogError(exOuter, "Error while processing RSS feeds.");
                }

                await Task.Delay(TimeSpan.FromHours(_rssFetchIntervalHours), cancellationToken);
            }
        }

        private static bool IsValidDescription(string? desc)
        {
            if (string.IsNullOrWhiteSpace(desc))
                return false;

            var cleaned = Regex.Replace(desc, "<.*?>", string.Empty).Trim();
            if (cleaned.Length < 100)
                return false;

            if (!cleaned.Contains('.'))
                return false;

            var lower = cleaned.ToLowerInvariant();
            if (lower.Contains("read more") || lower.Contains("continue reading") || lower.Contains("full story"))
                return false;

            return true;
        }

        private async Task<string> FetchAndExtractArticleAsync(string url, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(url))
                return string.Empty;

            var client = _httpClientFactory.CreateClient();
            client.Timeout = TimeSpan.FromSeconds(_fetchArticleTimeoutSeconds);

            string html;
            try
            {
                html = await client.GetStringAsync(url, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to download HTML from {Url}", url);
                return string.Empty;
            }

            try
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(html);

                var articleNode = doc.DocumentNode.SelectSingleNode("//article")
                               ?? doc.DocumentNode.SelectSingleNode("//div[contains(@class,'article') or contains(@class,'post') or contains(@id,'article')]");

                string text = string.Empty;
                if (articleNode != null)
                {
                    var nodes = articleNode.SelectNodes(".//p|.//h1|.//h2|.//h3");
                    if (nodes != null)
                        text = string.Join("\n\n", nodes.Select(n => HtmlEntity.DeEntitize(n.InnerText.Trim())));
                }

                if (string.IsNullOrWhiteSpace(text))
                {
                    var pNodes = doc.DocumentNode.SelectNodes("//p");
                    if (pNodes != null)
                    {
                        var top = pNodes.Select(p => HtmlEntity.DeEntitize(p.InnerText.Trim()))
                                        .Where(s => !string.IsNullOrWhiteSpace(s))
                                        .OrderByDescending(s => s.Length)
                                        .Take(6);
                        text = string.Join("\n\n", top);
                    }
                }

                text = CleanText(text);
                return text;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to parse HTML for {Url}", url);
                return string.Empty;
            }
        }

        private static string CleanText(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            var s = Regex.Replace(input, @"\s+", " ").Trim();
            s = s.Replace("\r", " ").Replace("\n", " ").Trim();

            return s;
        }

        private static string TruncateToSentenceBoundary(string text, int maxChars)
        {
            if (string.IsNullOrWhiteSpace(text) || text.Length <= maxChars)
                return text?.Trim() ?? string.Empty;

            var substr = text.Substring(0, maxChars);
            var lastDot = substr.LastIndexOf('.');
            if (lastDot > maxChars / 2)
                return substr.Substring(0, lastDot + 1).Trim();

            var lastSpace = substr.LastIndexOf(' ');
            if (lastSpace > 0)
                return substr.Substring(0, lastSpace).Trim();

            return substr.Trim();
        }

        private static List<List<T>> SplitList<T>(List<T> items, int size)
        {
            var res = new List<List<T>>();
            for (int i = 0; i < items.Count; i += size)
                res.Add(items.GetRange(i, Math.Min(size, items.Count - i)));

            return res;
        }

        private async Task SaveWithRetryAsync(AppDbContext db, string title, CancellationToken token)
        {
            const int maxRetries = 3;
            int delay = 1000;

            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                try
                {
                    await db.SaveChangesAsync(token);
                    return;
                }
                catch (DbUpdateException ex) when (attempt < maxRetries)
                {
                    _logger.LogWarning(ex, "Transient DB error saving '{Title}'. Retrying in {Delay}ms (attempt {Attempt}/{Max})", title, delay, attempt, maxRetries);
                    await Task.Delay(delay, token);
                    delay *= 2;
                }
            }

            _logger.LogError("Failed to save '{Title}' after {MaxRetries} retries.", title, maxRetries);
        }
    }
}