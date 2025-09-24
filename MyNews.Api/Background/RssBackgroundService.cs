using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using MyNews.Api.Data;
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
                    var sources = await dbContext.Sources.ToListAsync(cancellationToken);

                    foreach (var source in sources)
                    {
                        var rssItems = await rssService.FetchAndProcessRssFeedAsync(new[] { source });
                        var newsToAdd = new List<NewsItem>();

                        foreach (var rssItem in rssItems)
                        {
                            if (rssItem.PublishedAt < DateTime.UtcNow.AddDays(-7))
                                continue;

                            bool exists = await dbContext.NewsItems
                                .AnyAsync(n => n.Title == rssItem.Title && n.SourceId == source.Id, cancellationToken);

                            if (exists) continue;

                            var (summary, section) = await chatGptService.EnrichNewsAsync(rssItem.Title);

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

                            //_logger.LogInformation(
                            //    "Added {Count} new news items from source '{SourceName}' at {Time}",
                            //    newsToAdd.Count, source.Name, DateTime.UtcNow);
                        }
                        else
                        {
                            //_logger.LogInformation(
                            //    "No new items found for source '{SourceName}' at {Time}",
                            //    source.Name, DateTime.UtcNow);
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
