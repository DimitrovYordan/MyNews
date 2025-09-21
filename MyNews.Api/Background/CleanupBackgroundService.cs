using Microsoft.EntityFrameworkCore;

using MyNews.Api.Data;

namespace MyNews.Api.Background
{
    public class CleanupBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<CleanupBackgroundService> _logger;

        public CleanupBackgroundService(IServiceScopeFactory scopeFactory, ILogger<CleanupBackgroundService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    var cutoffDate = DateTime.UtcNow.AddDays(-2);

                    var oldNews = await dbContext.NewsItems
                        .Where(n => n.PublishedAt < cutoffDate ||
                                    dbContext.UserNewsReads.Any(ur => ur.NewsItemId == n.Id))
                        .ToListAsync(cancellationToken);

                    if (oldNews.Any())
                    {
                        dbContext.NewsItems.RemoveRange(oldNews);
                        await dbContext.SaveChangesAsync();

                        _logger.LogInformation("Deleted {Count} old/read news items ad {Time}", oldNews.Count, DateTime.UtcNow);
                    }
                }
                catch (Exception ex) 
                {
                    _logger.LogError(ex, "Error while cleaning up old/read news items.");
                }

                await Task.Delay(TimeSpan.FromDays(2), cancellationToken);
            }
        }
    }
}
