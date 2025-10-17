using MyNews.Api.Enums;

namespace MyNews.Api.Models
{
    public class Source
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Url { get; set; } = string.Empty;

        public string? LanguageCode { get; set; }

        public SectionType? Section { get; set; }

        public ICollection<NewsItem> NewsItems { get; set; } = new List<NewsItem>();
    }
}
