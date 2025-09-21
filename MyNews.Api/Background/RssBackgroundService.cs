using MyNews.Api.Interfaces;

namespace MyNews.Api.Background
{
    public class RssBackgroundService : BackgroundService
    {
        private readonly IRssService _rssService;

        public RssBackgroundService(IRssService rssService)
        {
            _rssService = rssService;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await _rssService.FetchAndProcessRssFeedAsync();
                await Task.Delay(TimeSpan.FromHours(6), cancellationToken);
            }
        }
    }
}
