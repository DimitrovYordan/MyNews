namespace MyNews.Api.Services
{
    public interface IJwtService
    {
        string GenerateToken(Guid userId, string email);
    }
}
