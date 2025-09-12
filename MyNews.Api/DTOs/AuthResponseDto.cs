namespace MyNews.Api.DTOs
{
    public class AuthResponseDto
    {
        public Guid UserId { get; set; } = Guid.NewGuid();

        public string Token { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;
    }
}
