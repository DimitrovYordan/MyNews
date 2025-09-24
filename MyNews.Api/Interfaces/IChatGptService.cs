using MyNews.Api.Enums;

namespace MyNews.Api.Interfaces
{
    public interface IChatGptService
    {
        Task<(string Summary, SectionType Section)> EnrichNewsAsync(string text);
    }
}
