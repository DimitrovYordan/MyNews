using System;

namespace MyNews.Api.Models
{
    // Represents a news article in the system
    public class NewsItem
    {
        public int Id { get; set; }             // Primary key

        public string Title { get; set; }       // News title

        public string Source { get; set; }      // Source of the news

        public DateTime Date { get; set; }      // Publication date
    }
}
