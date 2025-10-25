using MyNews.Api.DTOs;

namespace MyNews.Api.Interfaces
{
    public interface IUserService
    {
        Task<object?> GetProfileAsync(Guid userId);

        Task<string?> UpdateProfileAsync(Guid userId, UpdateUserDto updateUserDto);

        Task<string?> DeleteUserAsync(Guid userId);

        Task<bool> MarkOnboardingCompletedAsync(Guid userId);

        Task<bool?> GetOnboardingStatusAsync(Guid userId);
    }
}
