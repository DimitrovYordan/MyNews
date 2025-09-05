namespace MyNews.Api.DTOs
{
    public class SectionWithNewsDto
    {
        public int SectionId { get; set; }

        public string SectionName { get; set; }

        public List<NewsItemDto> News {  get; set; }
    }
}
