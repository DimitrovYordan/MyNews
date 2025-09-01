using System;

namespace MyNews.Api.Models
{
    // Represents a news article in the system
    public class NewsItem
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public DateTime PublishedAt { get; set; } = DateTime.UtcNow;

        // Foreign keys
        public int SectionId { get; set; }
        public Section? Section { get; set; }

        public int SourceId { get; set; }
        public Source? Source { get; set; }
    }
}
