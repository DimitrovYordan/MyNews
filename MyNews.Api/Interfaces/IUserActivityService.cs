namespace MyNews.Api.Interfaces
{
    public interface IUserActivityService
    {
        Task RecordLoginAsync(Guid userId);
    }
}
