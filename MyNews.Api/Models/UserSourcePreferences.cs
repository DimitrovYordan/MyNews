namespace MyNews.Api.Models
{
    public class UserSourcePreferences
    {
        public Guid UserId { get; set; }

        public User User { get; set; }

        public int SourceId { get; set; }

        public bool IsSelected { get; set; } = true;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
