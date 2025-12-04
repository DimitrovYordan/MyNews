namespace MyNews.Api.DTOs
{
    public class NewsFilterRequest
    {
        public List<int> SectionIds { get; set; } = new();

        public List<int> SourceIds { get; set; } = new();
    }
}
