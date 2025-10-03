using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations;

using MyNews.Api.Enums;

namespace MyNews.Api.Models
{
    // Represents a news article in the system
    [Index(nameof(Title), nameof(SourceId), IsUnique = true)]
    public class NewsItem
    {
        public Guid Id { get; set; }

        public SectionType Section { get; set; }

        [MaxLength(500)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Summary { get; set; } = string.Empty;

        public string Link { get; set; } = string.Empty;

        /// <summary>
        /// When the article was originally published on the source site.
        /// </summary>
        public DateTime PublishedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// When the system fetched and stored the article.
        /// </summary>
        public DateTime FetchedAt { get; set; } = DateTime.UtcNow;

        // Foreign keys
        public int SourceId { get; set; }
        public Source? Source { get; set; }

        public ICollection<UserNewsRead> UserReads { get; set; } = new List<UserNewsRead>();

        public virtual List<NewsTranslation> Translations { get; set; } = new();
    }
}
