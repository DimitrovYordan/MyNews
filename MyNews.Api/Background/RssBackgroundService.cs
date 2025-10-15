using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using MyNews.Api.Data;
using MyNews.Api.DTOs;
using MyNews.Api.Enums;
using MyNews.Api.Interfaces;
using MyNews.Api.Models;
using MyNews.Api.Options;

namespace MyNews.Api.Background
{
    public class RssBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<RssBackgroundService> _logger;
        private readonly int _rssFetchIntervalHours;
        private readonly int batchSize = 3;

        public RssBackgroundService(IServiceScopeFactory scopeFactory, ILogger<RssBackgroundService> logger, IOptions<BackgroundJobsOptions> options)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _rssFetchIntervalHours = options.Value.RssFetchIntervalHours;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("RSS background service started at {Time}", DateTime.UtcNow);

            while (!cancellationToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();

                var rssService = scope.ServiceProvider.GetRequiredService<IRssService>();
                var chatGptService = scope.ServiceProvider.GetRequiredService<IChatGptService>();
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                int totalNewsAdded = 0;
                int totalSourcesProcessed = 0;
                var batchStart = DateTime.UtcNow;

                try
                {
                    var sources = await dbContext.Sources.ToListAsync(cancellationToken);
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

                            var batches = SplitList(freshItems, batchSize);

                            using var semaphore = new SemaphoreSlim(3);
                            var batchTasks = new List<Task>();

                            foreach (var batch in batches)
                            {
                                await semaphore.WaitAsync(cancellationToken);

                                var task = Task.Run(async () =>
                                {
                                    try
                                    {
                                        var titles = batch.Select(f => f.Title).ToList();
                                        var enrichedResults = await chatGptService.EnrichBatchAsync(titles, cancellationToken);

                                        for (int i = 0; i < enrichedResults.Count; i++)
                                        {
                                            var enriched = enrichedResults[i];
                                            var rssItem = batch[i];

                                            if (enriched == null)
                                            {
                                                enriched = new EnrichedNewsDto
                                                {
                                                    Title = rssItem.Title,
                                                    Summary = "Summary unavailable",
                                                    Section = SectionType.General,
                                                    Translations = new Dictionary<string, NewsTranslationDto>()
                                                };
                                            }

                                            var newsItem = new NewsItem
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

                                            dbContext.NewsItems.Add(newsItem);

                                            foreach (var kv in enriched.Translations)
                                            {
                                                var lang = kv.Key;
                                                var tDto = kv.Value ?? new NewsTranslationDto { LanguageCode = lang, Title = string.Empty, Summary = string.Empty };

                                                dbContext.NewsTranslations.Add(new NewsTranslation
                                                {
                                                    Id = Guid.NewGuid(),
                                                    NewsItemId = newsItem.Id,
                                                    LanguageCode = tDto.LanguageCode ?? lang,
                                                    Title = tDto.Title ?? string.Empty,
                                                    Summary = tDto.Summary ?? string.Empty,
                                                    Section = enriched.Section
                                                });
                                            }
                                        }

                                        await dbContext.SaveChangesAsync(cancellationToken);
                                        Interlocked.Add(ref totalNewsAdded, enrichedResults.Count);

                                        _logger.LogInformation("[RSS] Saved {Count} enriched items for source {Url}", enrichedResults.Count, source.Url);
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.LogError(ex, "[RSS] Error while enriching batch for {Url}", source.Url);
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
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while processing RSS feeds.");
                }

                var batchEnd = DateTime.UtcNow;
                _logger.LogInformation("RSS batch finished at {Time}. Sources processed: {SourcesCount}. Total news added: {NewsCount}. Duration: {Duration}s",
                    batchEnd, totalSourcesProcessed, totalNewsAdded, (batchEnd - batchStart).TotalSeconds);

                await Task.Delay(TimeSpan.FromHours(_rssFetchIntervalHours), cancellationToken);
            }
        }

        private static List<List<T>> SplitList<T>(List<T> items, int size)
        {
            var result = new List<List<T>>();
            for (int i = 0; i < items.Count; i += size)
                result.Add(items.GetRange(i, Math.Min(size, items.Count - i)));
            return result;
        }
    }
}
