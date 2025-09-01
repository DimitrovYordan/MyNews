namespace MyNews.Api.Models
{
    public class Section
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public ICollection<NewsItem> NewsItems { get; set; } = new List<NewsItem>();
    }
}
