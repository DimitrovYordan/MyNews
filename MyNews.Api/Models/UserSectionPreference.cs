using MyNews.Api.Enums;

namespace MyNews.Api.Models
{
    public class UserSectionPreference
    {
        public Guid UserId { get; set; }

        public User User { get; set; }

        public SectionType SectionType { get; set; }

        public bool IsSelected { get; set; } = true;

        public int OrderIndex { get; set; }

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
