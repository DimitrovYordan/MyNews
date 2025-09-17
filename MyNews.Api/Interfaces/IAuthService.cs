using MyNews.Api.DTOs;

namespace MyNews.Api.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);

        Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);

        Task<bool> ForgotPasswordAsync(string email);

        Task<bool> ResetPasswordAsync(string token, string newPassword);
    }
}
