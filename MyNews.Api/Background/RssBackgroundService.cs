using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using MyNews.Api.Data;
using MyNews.Api.DTOs;
using MyNews.Api.Interfaces;
using MyNews.Api.Models;

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
                using var scope = _scopeFactory.CreateScope();

                var rssService = scope.ServiceProvider.GetRequiredService<IRssService>();
                var chatGptService = scope.ServiceProvider.GetRequiredService<IChatGptService>();
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                try
                {
                    var sources = await dbContext.Sources.Where(s => s.Id == 29).ToListAsync(cancellationToken);

                    foreach (var source in sources)
                    {
                        var rssItems = await rssService.FetchAndProcessRssFeedAsync(new[] { source });

                        var freshItems = new List<NewsItemDto>();
                        foreach (var rssItem in rssItems)
                        {
                            if (rssItem.PublishedAt < DateTime.UtcNow.AddDays(-5))
                            {
                                continue;
                            }

                            bool existsInDb = await dbContext.NewsItems
                                .AnyAsync(n => n.Title == rssItem.Title && n.SourceId == source.Id, cancellationToken);

                            bool existsInBatch = freshItems.Any(n => n.Title == rssItem.Title && n.SourceUrl == source.Url);

                            if (!existsInDb && !existsInBatch)
                            {
                                freshItems.Add(rssItem);
                            }
                        }

                        if (!freshItems.Any())
                        {
                            continue;
                        }

                        var titles = freshItems.Select(r => r.Title).ToList();
                        var enriched = await chatGptService.EnrichNewsBatchAsync(titles, cancellationToken);

                        var newsToAdd = new List<NewsItem>();
                        for (int i = 0; i < freshItems.Count; i++)
                        {
                            var rssItem = freshItems[i];
                            var (summary, section) = i < enriched.Count
                                ? enriched[i]
                                : ("Summary unavailable", Enums.SectionType.General);

                            var newsItem = new NewsItem
                            {
                                Id = Guid.NewGuid(),
                                Section = section,
                                Title = rssItem.Title,
                                Summary = summary,
                                Link = rssItem.Link,
                                PublishedAt = rssItem.PublishedAt,
                                FetchedAt = DateTime.UtcNow,
                                SourceId = source.Id
                            };

                            newsToAdd.Add(newsItem);
                        }

                        if (newsToAdd.Any())
                        {
                            dbContext.NewsItems.AddRange(newsToAdd);
                            await dbContext.SaveChangesAsync(cancellationToken);

                            _logger.LogInformation(
                                "Added {Count} new news items from source '{SourceName}' at {Time}",
                                newsToAdd.Count, source.Name, DateTime.UtcNow);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while processing RSS feeds.");
                }

                await Task.Delay(TimeSpan.FromHours(_rssFetchIntervalHours), cancellationToken);
            }
        }
    }
}
