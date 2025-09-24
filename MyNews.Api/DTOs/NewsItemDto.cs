using MyNews.Api.Enums;

namespace MyNews.Api.DTOs
{
    public class NewsItemDto
    {
        public SectionType Section { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Summary { get; set; } = string.Empty;

        public DateTime PublishedAt { get; set; }

        public string Link { get; set; } = string.Empty;

        public string SourceName { get; set; } = string.Empty;

        public string SourceUrl { get; set; } = string.Empty;

        public bool IsNew { get; set; }
    }
}
