namespace MyNews.Api.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(Guid userId, string email);
    }
}
