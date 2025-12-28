using System.Net;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using System.Xml;

using MyNews.Api.DTOs;
using MyNews.Api.Interfaces;
using MyNews.Api.Models;

namespace MyNews.Api.Services
{
    public class RssService : IRssService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<RssService> _logger;

        public RssService(HttpClient httpClient, ILogger<RssService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<List<NewsItemDto>> FetchAndProcessRssFeedAsync(IEnumerable<Source> sources)
        {
            var result = new List<NewsItemDto>();

            foreach (var source in sources)
            {
                try
                {
                    var raw = await _httpClient.GetStringAsync(source.Url);
                    var response = SanitizeRss(raw);

                    var settings = new XmlReaderSettings
                    {
                        DtdProcessing = DtdProcessing.Ignore
                    };

                    using var stringReader = new StringReader(response);
                    using var reader = XmlReader.Create(stringReader, settings);
                    var feed = SyndicationFeed.Load(reader);

                    foreach (var feedItem in feed.Items)
                    {
                        string description = string.Empty;

                        try
                        {
                            if (feedItem.Summary != null && !string.IsNullOrWhiteSpace(feedItem.Summary.Text))
                            {
                                description = System.Net.WebUtility.HtmlDecode(feedItem.Summary.Text).Trim();
                            }
                            else if (feedItem.Content is TextSyndicationContent textContent &&
                                    !string.IsNullOrWhiteSpace(textContent.Text))
                            {
                                description = System.Net.WebUtility.HtmlDecode(textContent.Text).Trim();
                            }
                            else
                            {
                                foreach (var ext in feedItem.ElementExtensions)
                                {
                                    try
                                    {
                                        var extObj = ext.GetObject<System.Xml.Linq.XElement>();
                                        if (extObj != null)
                                        {
                                            var val = extObj.Value;
                                            if (!string.IsNullOrWhiteSpace(val))
                                            {
                                                description = System.Net.WebUtility.HtmlDecode(val).Trim();
                                                if (!string.IsNullOrEmpty(description))
                                                    break;
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.LogError(ex.Message, "Unexpected error processing RSS for {SourceName}", ext);
                                    }
                                }
                            }

                            if (!string.IsNullOrWhiteSpace(description))
                            {
                                description = StripHtml(description);
                            }
                        }
                        catch
                        {
                            description = string.Empty;
                        }

                        var newsDto = new NewsItemDto
                        {
                            Title = feedItem.Title.Text,
                            Link = feedItem.Links
                                           .Select(l => l.Uri?.ToString())
                                           .FirstOrDefault(u => Uri.IsWellFormedUriString(u, UriKind.Absolute))
                                           ?? string.Empty,
                            PublishedAt = feedItem.PublishDate.UtcDateTime,
                            SourceName = source.Name,
                            SourceUrl = source.Url,
                            Description = description,
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

        private static string StripHtml(string input)
        {
            return System.Text.RegularExpressions.Regex.Replace(input, "<.*?>", string.Empty);
        }

        private string SanitizeRss(string xml)
        {
            // decode html entities
            xml = WebUtility.HtmlDecode(xml);

            // fix invalid href/src attributes
            xml = Regex.Replace(xml,
                @"(href|src)\s*=\s*[""']([^""']+)[""']",
                m =>
                {
                    var url = m.Groups[2].Value.Trim();

                    if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
                    {
                        try
                        {
                            url = Uri.EscapeUriString(url);
                        }
                        catch
                        {
                            return ""; // drop invalid attribute completely 
                        }
                    }

                    return $"{m.Groups[1].Value}=\"{url}\"";
                });

            return xml;
        }
    }
}
