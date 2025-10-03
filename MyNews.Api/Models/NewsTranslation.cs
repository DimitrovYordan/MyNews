using System.ComponentModel.DataAnnotations;

using MyNews.Api.Enums;

namespace MyNews.Api.Models
{
    public class NewsTranslation
    {
        public Guid Id { get; set; }

        public Guid NewsItemId { get; set; }

        [MaxLength(5)]
        public string LanguageCode { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public string Summary { get; set; }

        public SectionType Section {  get; set; }

        public virtual NewsItem NewsItem { get; set; } = null!;
    }
}
