namespace MyNews.Api.Interfaces
{
    public interface IChatGptService
    {
        Task<string> GenerateSummaryAsync(string text);
    }
}
