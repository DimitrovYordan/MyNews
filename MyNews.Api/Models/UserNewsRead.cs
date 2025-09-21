namespace MyNews.Api.Models
{
    public class UserNewsRead
    {
        public int Id { get; set; }

        public Guid NewsItemId { get; set; }
        public NewsItem NewsItem { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public DateTime ReadAt { get; set; } = DateTime.UtcNow;
    }
}
