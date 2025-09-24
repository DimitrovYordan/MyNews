namespace MyNews.Api.Models
{
    public class UserNewsRead
    {
        public Guid NewsItemId { get; set; }
        public NewsItem NewsItem { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        /// <summary>
        /// When the user read the summary of the news.
        /// </summary>
        public DateTime ReadAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// True if the user clicked the title.
        /// </summary>
        public bool HasClickedTitle { get; set; } = false;

        /// <summary>
        /// True if the user clicked the full article link.
        /// </summary>
        public bool HasClickedLink { get; set; } = false;
    }
}
