using MyNews.Api.Enums;

namespace MyNews.Api.DTOs
{
    public class EnrichedNewsDto
    {
        public string Summary { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public SectionType Section { get; set; } = SectionType.General;

        public Dictionary<string, NewsTranslationDto> Translations { get; set; } = new();
    }
}
