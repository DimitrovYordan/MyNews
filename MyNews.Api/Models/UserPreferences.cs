using MyNews.Api.Enums;

namespace MyNews.Api.Models
{
    public class UserPreferences
    {
        public Guid UserId { get; set; }

        public User User { get; set; }

        public SectionType? SectionType { get; set; }

        public int? SourceId { get; set; }

        public bool IsSelected { get; set; } = true;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
