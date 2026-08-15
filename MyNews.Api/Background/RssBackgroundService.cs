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
        private static readonly SemaphoreSlim _cycleLock = new SemaphoreSlim(1, 1);

        public RssBackgroundService(
            IServiceScopeFactory scopeFactory,
            ILogger<RssBackgroundService> logger,
            IOptions<BackgroundJobsOptions> bjOptions,
            IHttpClientFactory httpClientFactory,
            IOptions<LocalizationOptions> localizationOptions,
            IOptions<OpenAIOptions> aiOptions)
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
                if (await _cycleLock.WaitAsync(0, cancellationToken))
                {
                    try
                    {
                        var batchStart = DateTime.UtcNow;
                        List<Source> sources;

                        using (var scope = _scopeFactory.CreateScope())
                        {
                            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                            sources = await dbContext.Sources.AsNoTracking().ToListAsync(cancellationToken);
                        }

                        var sourceSemaphore = new SemaphoreSlim(5);

                        var sourceTasks = sources.Select(async source =>
                        {
                            await sourceSemaphore.WaitAsync(cancellationToken);
                            try
                            {
                                using var sourceCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                                sourceCts.CancelAfter(TimeSpan.FromMinutes(3));

                                await ProcessSingleSourceAsync(source, sourceCts.Token);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, "Error processing source {Url}", source.Url);
                            }
                            finally
                            {
                                sourceSemaphore.Release();
                            }
                        });

                        await Task.WhenAll(sourceTasks);

                        var batchEnd = DateTime.UtcNow;
                        _logger.LogInformation("RSS cycle finished at {Time}. Total sources: {SourcesCount}. Duration: {Duration}s",
                            batchEnd, sources.Count, (batchEnd - batchStart).TotalSeconds);
                    }
                    catch (Exception exOuter)
                    {
                        _logger.LogError(exOuter, "Error while processing RSS feeds cycle.");
                    }
                    finally
                    {
                        _cycleLock.Release();
                    }
                }
                else
                {
                    _logger.LogWarning("[RSS] Previous cycle is still running. Skipping current run to prevent overlap.");
                }

                await Task.Delay(TimeSpan.FromHours(_rssFetchIntervalHours), cancellationToken);
            }
        }

        private async Task ProcessSingleSourceAsync(Source source, CancellationToken cancellationToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var rssService = scope.ServiceProvider.GetRequiredService<IRssService>();
            var chatGptService = scope.ServiceProvider.GetRequiredService<IChatGptService>();

            _logger.LogInformation("[RSS] Starting processing for source: {Url}", source.Url);

            var rssItems = await rssService.FetchAndProcessRssFeedAsync(new[] { source });

            var potentialItems = rssItems
                .Where(i => i.PublishedAt >= DateTime.UtcNow.AddDays(-2) && !string.IsNullOrEmpty(i.Link))
                .ToList();

            if (!potentialItems.Any()) return;

            var rssLinks = potentialItems.Select(i => i.Link).ToList();

            var existingLinksInDb = await dbContext.NewsItems
                .AsNoTracking()
                .Where(n => n.SourceId == source.Id && rssLinks.Contains(n.Link))
                .Select(n => n.Link)
                .ToListAsync(cancellationToken);

            var freshItems = new List<NewsItemDto>();
            foreach (var rssItem in potentialItems)
            {
                var cleanLink = rssItem.Link.Length > 450 ? rssItem.Link.Substring(0, 450) : rssItem.Link;

                bool existsInDb = existingLinksInDb.Contains(cleanLink);
                bool existsInBatch = freshItems.Any(n => n.Link == cleanLink);

                if (!existsInDb && !existsInBatch)
                {
                    rssItem.Link = cleanLink;
                    rssItem.Title = SanitizeText(rssItem.Title, 500);
                    rssItem.Summary = SanitizeText(rssItem.Summary, 4000);
                    freshItems.Add(rssItem);
                }
            }

            if (!freshItems.Any()) return;

            var batches = SplitList(freshItems, _aiOptions.BatchSize);

            foreach (var batch in batches)
            {
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
                        }
                    }

                    if (string.IsNullOrWhiteSpace(snippet)) snippet = rssItem.Title;

                    snippet = TruncateToSentenceBoundary(snippet, _aiOptions.MaxContentChars);
                    inputs.Add(new NewsForEnrichmentDto
                    {
                        Title = rssItem.Title,
                        ContentSnippet = snippet,
                        Link = rssItem.Link
                    });
                }

                var enrichedResults = await chatGptService.EnrichBatchAsync(inputs, cancellationToken, _globalTargetLanguages.ToList());
                
                _logger.LogInformation("[DEBUG ENRICHMENT OUTPUT]\n{Json}",
                    System.Text.Json.JsonSerializer.Serialize(enrichedResults, new System.Text.Json.JsonSerializerOptions { WriteIndented = true }));

                foreach (var (enriched, rssItem) in enrichedResults.Zip(batch))
                {
                    var newsItem = new NewsItem
                    {
                        Id = Guid.NewGuid(),
                        Section = enriched.Section,
                        Title = SanitizeText(enriched.Title, 500),
                        Summary = SanitizeText(enriched.Summary, 4000),
                        Link = SanitizeText(rssItem.Link, 450),
                        PublishedAt = rssItem.PublishedAt,
                        FetchedAt = DateTime.UtcNow,
                        SourceId = source.Id,
                        Translations = new List<NewsTranslation>()
                    };

                    dbContext.NewsItems.Add(newsItem);

                    var srcLang = enriched.SourceLanguage.ToLowerInvariant();
                    var targetLangs = new List<string>();

                    if (srcLang == "en" || srcLang == "eng")
                    {
                        targetLangs.Add("bul_Cyrl");
                    }
                    else if (srcLang == "bg" || srcLang == "bul")
                    {
                        targetLangs.Add("eng_Latn");
                    }
                    else
                    {
                        targetLangs.Add("bul_Cyrl");
                        targetLangs.Add("eng_Latn");
                    }
                    
                    _logger.LogInformation("[DEBUG TRANSLATION INPUT] Title: '{Title}' | SrcLang: '{SrcLang}' (Mapped: '{MappedSrc}') | Targets: [{Targets}]",
                        enriched.Title,
                        srcLang,
                        MapToNllbCode(srcLang),
                        string.Join(", ", targetLangs));

                    try
                    {
                        var nllbTranslations = await chatGptService.TranslateWithNllbAsync(
                            enriched.Title,
                            enriched.Summary,
                            MapToNllbCode(srcLang),
                            targetLangs,
                            cancellationToken);
                        
                        _logger.LogInformation("[DEBUG TRANSLATION OUTPUT RAW]\n{Json}",
                            System.Text.Json.JsonSerializer.Serialize(nllbTranslations, new System.Text.Json.JsonSerializerOptions { WriteIndented = true }));

                        foreach (var kv in nllbTranslations)
                        {
                            var langCode = NormalizeDbLang(kv.Key);
                            var tDto = kv.Value;

                            if (string.IsNullOrEmpty(tDto.Title)) continue;

                            newsItem.Translations.Add(new NewsTranslation
                            {
                                Id = Guid.NewGuid(),
                                NewsItemId = newsItem.Id,
                                LanguageCode = langCode,
                                Title = tDto.Title,
                                Summary = tDto.Summary,
                                Section = newsItem.Section
                            });
                        }
                        
                        var mappedTranslationsLog = newsItem.Translations.Select(t => new {
                            OriginalKey = t.LanguageCode,
                            SavedTitle = t.Title,
                            SavedSummaryLength = t.Summary?.Length ?? 0
                        });

                        _logger.LogInformation("[DEBUG FINAL TRANSLATIONS TO DB]\n{Json}",
                            System.Text.Json.JsonSerializer.Serialize(mappedTranslationsLog, new System.Text.Json.JsonSerializerOptions { WriteIndented = true }));
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to process translations for {Title}", enriched.Title);
                    }
                }

                await dbContext.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("[RSS] Saved batch of {Count} items for source {Url}", enrichedResults.Count, source.Url);
            }
        }

        private static bool IsValidDescription(string? desc)
        {
            if (string.IsNullOrWhiteSpace(desc)) return false;
            var cleaned = Regex.Replace(desc, "<.*?>", string.Empty).Trim();
            return cleaned.Length >= 100 && cleaned.Contains('.') &&
                   !cleaned.ToLowerInvariant().Contains("read more");
        }

        private async Task<string> FetchAndExtractArticleAsync(string url, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(url)) return string.Empty;
            var client = _httpClientFactory.CreateClient();
            client.Timeout = TimeSpan.FromSeconds(_fetchArticleTimeoutSeconds);

            try
            {
                var html = await client.GetStringAsync(url, cancellationToken);
                var doc = new HtmlDocument();
                doc.LoadHtml(html);

                var articleNode = doc.DocumentNode.SelectSingleNode("//article")
                               ?? doc.DocumentNode.SelectSingleNode("//div[contains(@class,'article') or contains(@class,'post')]");

                string text = string.Empty;
                if (articleNode != null)
                {
                    var nodes = articleNode.SelectNodes(".//p|.//h1|.//h2|.//h3");
                    if (nodes != null)
                        text = string.Join("\n\n", nodes.Select(n => HtmlEntity.DeEntitize(n.InnerText.Trim())));
                }

                return Regex.Replace(text, @"\s+", " ").Trim();
            }
            catch
            {
                return string.Empty;
            }
        }

        private static string TruncateToSentenceBoundary(string text, int maxChars)
        {
            if (string.IsNullOrWhiteSpace(text) || text.Length <= maxChars) return text?.Trim() ?? string.Empty;
            var substr = text.Substring(0, maxChars);
            var lastDot = substr.LastIndexOf('.');
            return lastDot > maxChars / 2 ? substr.Substring(0, lastDot + 1).Trim() : substr.Trim();
        }

        private static List<List<T>> SplitList<T>(List<T> items, int size)
        {
            var res = new List<List<T>>();
            for (int i = 0; i < items.Count; i += size)
                res.Add(items.GetRange(i, Math.Min(size, items.Count - i)));
            return res;
        }

        private string MapToNllbCode(string isoCode) => isoCode switch
        {
            "bg" or "bul" => "bul_Cyrl",
            "en" or "eng" => "eng_Latn",
            "de" or "deu" => "deu_Latn",
            "fr" or "fra" => "fra_Latn",
            "es" or "spa" => "spa_Latn",
            "ru" or "rus" => "rus_Cyrl",
            _ => isoCode + "_Latn"
        };

        private static string NormalizeDbLang(string nllbCode)
        {
            if (string.IsNullOrWhiteSpace(nllbCode)) return "EN";
            var code = nllbCode.Split('_')[0].ToLower();
            return code switch
            {
                "eng" or "en" => "EN",
                "bul" or "bg" => "BG",
                "deu" or "de" => "DE",
                "fra" or "fr" => "FR",
                _ => code.ToUpper()
            };
        }

        private string SanitizeText(string text, int maxLength = 450)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            var decoded = System.Net.WebUtility.HtmlDecode(text);
            var clean = Regex.Replace(decoded, @"[^\u0020-\u007E\u0400-\u04FF\u2010-\u2015\s]", "");
            clean = new string(clean.Where(c => !char.IsControl(c)).ToArray()).Trim();
            return clean.Length > maxLength ? clean.Substring(0, maxLength) : clean;
        }
    }
}