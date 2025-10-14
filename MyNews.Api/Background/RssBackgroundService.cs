//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Options;

//using MyNews.Api.Data;
//using MyNews.Api.DTOs;
//using MyNews.Api.Enums;
//using MyNews.Api.Interfaces;
//using MyNews.Api.Models;
//using MyNews.Api.Options;

//namespace MyNews.Api.Background
//{
//    public class RssBackgroundService : BackgroundService
//    {
//        private readonly IServiceScopeFactory _scopeFactory;
//        private readonly ILogger<RssBackgroundService> _logger;
//        private readonly int _rssFetchIntervalHours;

//        public RssBackgroundService(IServiceScopeFactory scopeFactory, ILogger<RssBackgroundService> logger, IOptions<BackgroundJobsOptions> options)
//        {
//            _scopeFactory = scopeFactory;
//            _logger = logger;
//            _rssFetchIntervalHours = options.Value.RssFetchIntervalHours;
//        }

//        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
//        {
//            while (!cancellationToken.IsCancellationRequested)
//            {
//                using var scope = _scopeFactory.CreateScope();

//                var rssService = scope.ServiceProvider.GetRequiredService<IRssService>();
//                var chatGptService = scope.ServiceProvider.GetRequiredService<IChatGptService>();
//                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

//                var newsToAdd = new List<NewsItem>();
//                var translationsToAdd = new List<NewsTranslation>();

//                int totalNewsAdded = 0;
//                int totalSourcesProcessed = 0;

//                var batchStartTime = DateTime.UtcNow;
//                _logger.LogInformation("RSS batch started at {Time}", batchStartTime);

//                try
//                {
//                    var sources = await dbContext.Sources.ToListAsync(cancellationToken);
//                    int totalSources = sources.Count;
//                    int index = 1;

//                    foreach (var source in sources)
//                    {
//                        totalSourcesProcessed++;
//                        _logger.LogInformation($"[RSS] Processing source {index}/{totalSources}: {source.Url}");

//                        try
//                        {
//                            var rssItems = await rssService.FetchAndProcessRssFeedAsync(new[] { source });

//                            var freshItems = new List<NewsItemDto>();
//                            foreach (var rssItem in rssItems)
//                            {
//                                if (rssItem.PublishedAt < DateTime.UtcNow.AddDays(-2))
//                                    continue;

//                                bool existsInDb = await dbContext.NewsItems
//                                    .AnyAsync(n => n.Title == rssItem.Title && n.SourceId == source.Id, cancellationToken);

//                                bool existsInBatch = freshItems.Any(n => n.Title == rssItem.Title && n.SourceUrl == source.Url);

//                                if (!existsInDb && !existsInBatch)
//                                    freshItems.Add(rssItem);
//                            }

//                            if (!freshItems.Any())
//                            {
//                                index++;
//                                continue;
//                            }

//                            var titles = freshItems.Select(r => r.Title).ToList();
//                            var enriched = await chatGptService.EnrichNewsBatchAsync(titles, cancellationToken);

//                            for (int i = 0; i < freshItems.Count; i++)
//                            {
//                                var rssItem = freshItems[i];
//                                EnrichedNewsDto item = i < enriched.Count
//                                    ? enriched[i]
//                                    : new EnrichedNewsDto
//                                    {
//                                        Title = rssItem.Title,
//                                        Summary = "Summary unavailable",
//                                        Section = SectionType.General
//                                    };

//                                var newsItem = new NewsItem
//                                {
//                                    Id = Guid.NewGuid(),
//                                    Section = item.Section,
//                                    Title = item.Title,
//                                    Summary = item.Summary,
//                                    Link = rssItem.Link,
//                                    PublishedAt = rssItem.PublishedAt,
//                                    FetchedAt = DateTime.UtcNow,
//                                    SourceId = source.Id
//                                };

//                                newsToAdd.Add(newsItem);

//                                foreach (var translatedText in item.Translations)
//                                {
//                                    translationsToAdd.Add(new NewsTranslation
//                                    {
//                                        Id = Guid.NewGuid(),
//                                        NewsItemId = newsItem.Id,
//                                        LanguageCode = translatedText.Key,
//                                        Title = translatedText.Value.Title,
//                                        Summary = translatedText.Value.Summary,
//                                        Section = item.Section,
//                                    });
//                                }
//                            }

//                            if (newsToAdd.Any())
//                            {
//                                dbContext.NewsItems.AddRange(newsToAdd);
//                                dbContext.NewsTranslations.AddRange(translationsToAdd);
//                                await dbContext.SaveChangesAsync(cancellationToken);

//                                totalNewsAdded += newsToAdd.Count;
//                                newsToAdd.Clear();
//                                translationsToAdd.Clear();
//                            }
//                        }
//                        catch (Exception ex)
//                        {
//                            _logger.LogError(ex, $"Error while processing source: {source.Url}");
//                        }

//                        index++;
//                    }
//                }
//                catch (Exception ex)
//                {
//                    _logger.LogError(ex, "Error while processing RSS feeds.");
//                }

//                var batchEndTime = DateTime.UtcNow;
//                _logger.LogInformation("RSS batch finished at {Time}. Sources processed: {SourcesCount}. Total news added: {NewsCount}", batchEndTime, totalSourcesProcessed, totalNewsAdded);

//                await Task.Delay(TimeSpan.FromHours(_rssFetchIntervalHours), cancellationToken);
//            }
//        }
//    }
//}


using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MyNews.Api.Data;
using MyNews.Api.DTOs;
using MyNews.Api.Enums;
using MyNews.Api.Interfaces;
using MyNews.Api.Models;
using MyNews.Api.Options;
using System.Diagnostics;

namespace MyNews.Api.Background
{
    public class RssBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<RssBackgroundService> _logger;
        private readonly int _rssFetchIntervalHours;

        public RssBackgroundService(IServiceScopeFactory scopeFactory, ILogger<RssBackgroundService> logger, IOptions<BackgroundJobsOptions> options)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _rssFetchIntervalHours = options.Value.RssFetchIntervalHours;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var batchStopwatch = Stopwatch.StartNew();
                _logger.LogInformation("RSS background job started at {Time}", DateTime.UtcNow);

                using var scope = _scopeFactory.CreateScope();

                var rssService = scope.ServiceProvider.GetRequiredService<IRssService>();
                var chatGptService = scope.ServiceProvider.GetRequiredService<IChatGptService>();
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                int totalNewsAdded = 0;
                int totalSourcesProcessed = 0;

                try
                {
                    var sources = await dbContext.Sources.ToListAsync(cancellationToken);
                    int totalSources = sources.Count;
                    _logger.LogInformation("Total RSS sources to process: {Count}", totalSources);

                    for (int idx = 0; idx < sources.Count; idx++)
                    {
                        if (cancellationToken.IsCancellationRequested)
                        {
                            _logger.LogInformation("Cancellation requested, stopping RSS processing loop.");
                            break;
                        }

                        var source = sources[idx];
                        totalSourcesProcessed++;
                        _logger.LogInformation("[RSS] Processing source {Index}/{Total}: {Url}", idx + 1, totalSources, source.Url);

                        var sourceStopwatch = Stopwatch.StartNew();

                        try
                        {
                            List<NewsItemDto> rssItems;
                            try
                            {
                                rssItems = (await rssService.FetchAndProcessRssFeedAsync(new[] { source }))?.ToList() ?? new List<NewsItemDto>();
                                _logger.LogDebug("[RSS] Fetched {Count} items from {Url}", rssItems.Count, source.Url);
                            }
                            catch (Exception exFetch)
                            {
                                _logger.LogError(exFetch, "[RSS] Failed to fetch or parse feed for {Url}", source.Url);
                                continue;
                            }

                            var freshItems = new List<NewsItemDto>();
                            int skippedOld = 0;
                            int skippedExisting = 0;
                            foreach (var rssItem in rssItems)
                            {
                                if (rssItem.PublishedAt < DateTime.UtcNow.AddDays(-2))
                                {
                                    skippedOld++;
                                    continue;
                                }

                                bool existsInDb = await dbContext.NewsItems
                                    .AnyAsync(n => n.Title == rssItem.Title && n.SourceId == source.Id, cancellationToken);

                                bool existsInBatch = freshItems.Any(n => n.Title == rssItem.Title && n.SourceUrl == source.Url);

                                if (!existsInDb && !existsInBatch)
                                    freshItems.Add(rssItem);
                                else
                                    skippedExisting++;
                            }

                            _logger.LogInformation("[RSS] Source {Url} fetched {FetchedCount} items, skippedOld={SkippedOld}, skippedExisting={SkippedExisting}, kept={KeptCount}",
                                source.Url, rssItems.Count, skippedOld, skippedExisting, freshItems.Count);

                            if (!freshItems.Any())
                            {
                                sourceStopwatch.Stop();
                                _logger.LogDebug("[RSS] No fresh items for {Url}. Time: {ms} ms", source.Url, sourceStopwatch.ElapsedMilliseconds);
                                continue;
                            }

                            var titles = freshItems.Select(r => r.Title).ToList();
                            _logger.LogInformation("[RSS] Sending {Count} titles to enrichment for source {Url}", titles.Count, source.Url);

                            var enrichStopwatch = Stopwatch.StartNew();
                            List<EnrichedNewsDto> enriched;
                            try
                            {
                                enriched = await chatGptService.EnrichNewsBatchAsync(titles, cancellationToken);
                            }
                            catch (Exception exEnrich)
                            {
                                enrichStopwatch.Stop();
                                _logger.LogError(exEnrich, "[GPT] Enrichment failed for source {Url} after {Elapsed} ms", source.Url, enrichStopwatch.ElapsedMilliseconds);
                                enriched = titles.Select(t => new EnrichedNewsDto
                                {
                                    Title = t,
                                    Summary = "Summary unavailable",
                                    Section = SectionType.General
                                }).ToList();
                            }
                            enrichStopwatch.Stop();
                            _logger.LogInformation("[GPT] Enrichment for source {Url} finished in {Elapsed} ms, enrichedCount={Count}", source.Url, enrichStopwatch.ElapsedMilliseconds, enriched?.Count ?? 0);

                            var newsToAdd = new List<NewsItem>();
                            var translationsToAdd = new List<NewsTranslation>();
                            for (int i = 0; i < freshItems.Count; i++)
                            {
                                var rssItem = freshItems[i];
                                EnrichedNewsDto item = i < enriched.Count
                                    ? enriched[i]
                                    : new EnrichedNewsDto
                                    {
                                        Title = rssItem.Title,
                                        Summary = "Summary unavailable",
                                        Section = SectionType.General
                                    };

                                var newsItem = new NewsItem
                                {
                                    Id = Guid.NewGuid(),
                                    Section = item.Section,
                                    Title = item.Title,
                                    Summary = item.Summary,
                                    Link = rssItem.Link,
                                    PublishedAt = rssItem.PublishedAt,
                                    FetchedAt = DateTime.UtcNow,
                                    SourceId = source.Id
                                };

                                newsToAdd.Add(newsItem);

                                foreach (var translatedText in item.Translations)
                                {
                                    translationsToAdd.Add(new NewsTranslation
                                    {
                                        Id = Guid.NewGuid(),
                                        NewsItemId = newsItem.Id,
                                        LanguageCode = translatedText.Key,
                                        Title = translatedText.Value.Title,
                                        Summary = translatedText.Value.Summary,
                                        Section = item.Section,
                                    });
                                }
                            }

                            if (newsToAdd.Any())
                            {
                                var dbStopwatch = Stopwatch.StartNew();
                                try
                                {
                                    dbContext.NewsItems.AddRange(newsToAdd);
                                    dbContext.NewsTranslations.AddRange(translationsToAdd);
                                    await dbContext.SaveChangesAsync(cancellationToken);
                                    dbStopwatch.Stop();

                                    totalNewsAdded += newsToAdd.Count;
                                    _logger.LogInformation("[DB] Saved {Count} news and {TransCount} translations for source {Url} in {Elapsed} ms",
                                        newsToAdd.Count, translationsToAdd.Count, source.Url, dbStopwatch.ElapsedMilliseconds);
                                }
                                catch (Exception exDb)
                                {
                                    dbStopwatch.Stop();
                                    _logger.LogError(exDb, "[DB] Failed to save news for source {Url} after {Elapsed} ms", source.Url, dbStopwatch.ElapsedMilliseconds);
                                }
                            }
                        }
                        catch (Exception exSource)
                        {
                            _logger.LogError(exSource, "[RSS] Unexpected error while processing source {Url}", source.Url);
                        }
                        finally
                        {
                            sourceStopwatch.Stop();
                            _logger.LogDebug("[RSS] Finished source {Url} in {Elapsed} ms", source.Url, sourceStopwatch.ElapsedMilliseconds);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "[RSS] Fatal error while processing RSS feeds.");
                }
                finally
                {
                    batchStopwatch.Stop();
                    _logger.LogInformation("[RSS] Batch finished. Sources processed: {SourcesProcessed}. Total news added: {NewsAdded}. Batch time: {Elapsed} ms",
                        totalSourcesProcessed, totalNewsAdded, batchStopwatch.ElapsedMilliseconds);
                }

                try
                {
                    _logger.LogDebug("[RSS] Sleeping for {Hours} hours before next run", _rssFetchIntervalHours);
                    await Task.Delay(TimeSpan.FromHours(_rssFetchIntervalHours), cancellationToken);
                }
                catch (TaskCanceledException)
                {
                    _logger.LogInformation("[RSS] Delay cancelled, stopping background service.");
                }
            }
        }
    }
}
