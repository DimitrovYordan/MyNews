namespace MyNews.Api.Models
{
    public class UserActivity
    {
        public Guid UserId { get; set; }

        public int LoginCount { get; set; }

        public DateTime LastLoginUtc { get; set; }

        public User User { get; set; } = null!;
    }
}
