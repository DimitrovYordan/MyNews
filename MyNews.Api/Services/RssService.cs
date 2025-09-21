using MyNews.Api.DTOs;
using MyNews.Api.Interfaces;
using MyNews.Api.Models;

using System.ServiceModel.Syndication;
using System.Xml;

namespace MyNews.Api.Services
{
    public class RssService : IRssService
    {
        private readonly HttpClient _httpClient;
        private readonly INewsService _newsService;
        private readonly IChatGptService _chatGptService;
        private readonly ISourceService _sourceService;
        private readonly ILogger<RssService> _logger;

        public RssService(HttpClient httpClient, INewsService newsService, IChatGptService chatGptService, ISourceService sourceService, ILogger<RssService> logger)
        {
            _httpClient = httpClient;
            _newsService = newsService;
            _chatGptService = chatGptService;
            _sourceService = sourceService;
            _logger = logger;
        }

        public async Task<List<NewsItemDto>> FetchAndProcessRssFeedAsync()
        {
            var result = new List<NewsItemDto>();

            foreach (var item in _rssSources)
            {
                var sourceName = item.Key;
                var url = item.Value;

                try
                {
                    var source = await _sourceService.GetByNameAsync(sourceName);
                    if (source == null)
                    {
                        Console.WriteLine($"Source {sourceName} not found in database, skipping.");
                        continue;
                    }

                    var response = await _httpClient.GetStringAsync(url);

                    using var reader = XmlReader.Create(new StringReader(response));
                    var feed = SyndicationFeed.Load(reader);

                    foreach (var feedItem in feed.Items)
                    {
                        bool isNewsExists = await _newsService.ExistsByTitleAndSourceAsync(feedItem.Title.Text, source.Id);

                        var hasValidDate = feedItem.PublishDate != DateTimeOffset.MinValue;

                        var summaryText = feedItem.Summary?.Text ?? string.Empty;

                        var newsDto = new NewsItemDto
                        {
                            Title = feedItem.Title.Text,
                            Link = feedItem.Links[0].Uri.ToString(),
                            PublishedAt = feedItem.PublishDate.UtcDateTime,
                            SourceName = source.Name,
                            SourceUrl = source.Url,
                            Summary = await _chatGptService.GenerateSummaryAsync(feedItem.Title.Text + " " + summaryText),
                            IsNew = !isNewsExists
                        };

                        if (!isNewsExists)
                        {
                            var newsItem = new NewsItem
                            {
                                Id = Guid.NewGuid(),
                                Title = feedItem.Title.Text,
                                PublishedAt = newsDto.PublishedAt,
                                SourceId = source.Id,
                                Source = source
                            };

                            await _newsService.AddNewsItemAsync(newsItem);
                        }

                        result.Add(newsDto);
                    }
                }
                catch (HttpRequestException ex)
                {
                    _logger.LogError(ex, "HTTP error fetching RSS for {SourceName}", sourceName);
                    continue;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unexpected error processing RSS for {SourceName}", sourceName);
                    continue;
                }
            }

            return result;
        }

        private readonly Dictionary<string, string> _rssSources = new()
        {
            { "Dnevnik", "https://www.dnevnik.bg/rss/" },
            { "Investor", "https://www.investor.bg/rss/" },
            { "Vesti", "https://www.vesti.bg/rss-novini-3405031" }
        };
    }
}
