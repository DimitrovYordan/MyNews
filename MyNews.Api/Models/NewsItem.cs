using MyNews.Api.Enums;

namespace MyNews.Api.Models
{
    // Represents a news article in the system
    public class NewsItem
    {
        public Guid Id { get; set; }

        public SectionType Section { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Summary { get; set; } = string.Empty;

        public string Link { get; set; } = string.Empty;

        public DateTime PublishedAt { get; set; } = DateTime.UtcNow;

        // Foreign keys
        public int SourceId { get; set; }
        public Source? Source { get; set; }
    }
}
