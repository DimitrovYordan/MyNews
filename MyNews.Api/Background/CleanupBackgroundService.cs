using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using MyNews.Api.Data;
using MyNews.Api.Options;

namespace MyNews.Api.Background
{
    public class CleanupBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<CleanupBackgroundService> _logger;
        private readonly int _cleanupIntervalDays;

        public CleanupBackgroundService(IServiceScopeFactory scopeFactory, ILogger<CleanupBackgroundService> logger, IOptions<BackgroundJobsOptions> options)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _cleanupIntervalDays = options.Value.CleanupIntervalDays;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    await CleanupDeletedUsersPreferencesAsync(dbContext, cancellationToken);
                    await CleanupOldNewsAsync(dbContext, cancellationToken, daysBack: _cleanupIntervalDays);
                    await CleanupOldTranslationsAsync(dbContext, cancellationToken, daysBack: _cleanupIntervalDays);
                }
                catch (Exception ex) 
                {
                    _logger.LogError(ex, "Error while running cleanup tasks.");
                }

                await Task.Delay(TimeSpan.FromDays(1), cancellationToken);
            }
        }

        /// <summary>
        /// Delete old news and related UserNewsReads.
        /// </summary>
        private async Task CleanupOldNewsAsync(AppDbContext appDbContext, CancellationToken cancellationToken, int daysBack)
        {
            var cutofDate = DateTime.UtcNow.AddDays(-daysBack);

            var oldNews = await appDbContext.NewsItems
                .Where(n => n.PublishedAt < cutofDate)
                .ToListAsync(cancellationToken);

            if (oldNews.Any())
            {
                var newsIds = oldNews.Select(n => n.Id).ToList();

                var relatedReads = await appDbContext.UserNewsReads
                    .Where(r => newsIds.Contains(r.NewsItemId))
                    .ToListAsync(cancellationToken);

                if (relatedReads.Any())
                {
                    appDbContext.UserNewsReads.RemoveRange(relatedReads);

                    _logger.LogInformation(
                        "Deleted {Count} UserNewsReads for old news at {Time}",
                        relatedReads.Count, DateTime.UtcNow);
                }

                appDbContext.NewsItems.RemoveRange(oldNews);
                await appDbContext.SaveChangesAsync(cancellationToken);

                _logger.LogInformation(
                    "Deleted {Count} old news items at {Time}",
                    oldNews.Count, DateTime.UtcNow);
            }
        }

        /// <summary>
        /// Delete UserSectionPreferences of users marked as deleted.
        /// </summary>
        private async Task CleanupDeletedUsersPreferencesAsync(AppDbContext appDbContext, CancellationToken cancellationToken)
        {
            var deletedUserIds = await appDbContext.Users
                .IgnoreQueryFilters()
                .Where(u => u.IsDeleted)
                .Select(u => u.Id)
                .ToListAsync(cancellationToken);

            var oldPreferences = await appDbContext.UserPreferences
                .Where(p => deletedUserIds.Contains(p.UserId))
                .ToListAsync(cancellationToken);

            if (oldPreferences.Any())
            {
                appDbContext.UserPreferences.RemoveRange(oldPreferences);
                await appDbContext.SaveChangesAsync(cancellationToken);

                _logger.LogInformation(
                    "Deleted {Count} UserSectionPreferences for soft-deleted users at {Time}",
                    oldPreferences.Count, DateTime.UtcNow);
            }
        }

        /// <summary>
        /// Delete translations linked to news older than N days.
        /// </summary>
        private async Task CleanupOldTranslationsAsync(AppDbContext appDbContext, CancellationToken cancellationToken, int daysBack)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-daysBack);

            var oldTranslations = await appDbContext.NewsTranslations
                .Where(t => t.NewsItem.PublishedAt < cutoffDate)
                .ToListAsync(cancellationToken);

            if (oldTranslations.Any())
            {
                appDbContext.NewsTranslations.RemoveRange(oldTranslations);
                await appDbContext.SaveChangesAsync(cancellationToken);

                _logger.LogInformation(
                    "Deleted {Count} NewsTranslations older than {Days} days at {Time}",
                    oldTranslations.Count, _cleanupIntervalDays, DateTime.UtcNow);
            }
        }
    }
}
