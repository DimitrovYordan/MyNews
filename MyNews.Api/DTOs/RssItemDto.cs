namespace MyNews.Api.DTOs
{
    public class RssItemDto
    {
        public string Title { get; set; } = string.Empty;

        public string Link { get; set; } = string.Empty;

        public DateTime PublishedAt { get; set; }

        public int SourceId { get; set; }
    }
}
