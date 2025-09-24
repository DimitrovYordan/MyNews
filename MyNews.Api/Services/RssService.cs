using System.ServiceModel.Syndication;
using System.Xml;

using MyNews.Api.DTOs;
using MyNews.Api.Interfaces;
using MyNews.Api.Models;

namespace MyNews.Api.Services
{
    public class RssService : IRssService
    {
        private readonly HttpClient _httpClient;
        //private readonly INewsService _newsService;
        private readonly ILogger<RssService> _logger;

        public RssService(HttpClient httpClient, ILogger<RssService> logger)
        {
            _httpClient = httpClient;
            //_newsService = newsService;
            _logger = logger;
        }

        public async Task<List<NewsItemDto>> FetchAndProcessRssFeedAsync(IEnumerable<Source> sources)
        {
            var result = new List<NewsItemDto>();

            foreach (var source in sources)
            {
                try
                {
                    var response = await _httpClient.GetStringAsync(source.Url);

                    var settings = new XmlReaderSettings
                    {
                        DtdProcessing = DtdProcessing.Ignore
                    };

                    using var stringReader = new StringReader(response);
                    using var reader = XmlReader.Create(stringReader, settings);
                    var feed = SyndicationFeed.Load(reader);

                    foreach (var feedItem in feed.Items)
                    {
                        var newsDto = new NewsItemDto
                        {
                            Title = feedItem.Title.Text,
                            Link = feedItem.Links.FirstOrDefault()?.Uri.ToString() ?? string.Empty,
                            PublishedAt = feedItem.PublishDate.UtcDateTime,
                            SourceName = source.Name,
                            SourceUrl = source.Url,
                            Summary = string.Empty,
                            Section = 0,
                            IsNew = false
                        };

                        result.Add(newsDto);
                    }
                }
                catch (HttpRequestException ex)
                {
                    _logger.LogError(ex, "HTTP error fetching RSS for {SourceName}", source.Name);
                    continue;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unexpected error processing RSS for {SourceName}", source.Name);
                    continue;
                }
            }

            return result;
        }
    }
}
