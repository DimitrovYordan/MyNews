using Microsoft.EntityFrameworkCore;

using MyNews.Api.Data;
using MyNews.Api.DTOs;
using MyNews.Api.Interfaces;

namespace MyNews.Api.Services
{
    /// <summary>
    /// Provides user management operations such as profile retrieval, update, and deletion.
    /// </summary>
    public class UsersService : IUserService
    {
        public readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersService"/> class.
        /// </summary>
        /// <param name="context">Database context for accessing user data.</param>
        public UsersService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves the profile details of a user by ID.
        /// </summary>
        /// <param name="userId">Unique identifier of the user.</param>
        /// <returns>Returns an anonymous object with profile data or null if user not found.</returns>
        public async Task<object?> GetProfileAsync(Guid userId)
        {
            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId && !u.IsDeleted);
            if (user == null)
            {
                return null;
            }

            return new
            {
                user.FirstName,
                user.LastName,
                user.Email,
                user.Country,
                user.City
            };
        }

        /// <summary>
        /// Updates an existing user's profile information.
        /// </summary>
        /// <param name="userId">Unique identifier of the user.</param>
        /// <param name="updateUserDto">Data transfer object with updated user details.</param>
        /// <returns>
        /// Returns a success message string, or a specific error message such as 
        /// "Email is already in use" or "Passwords do not match."
        /// </returns>
        public async Task<string?> UpdateProfileAsync(Guid userId, UpdateUserDto updateUserDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId && !u.IsDeleted);
            if (user == null) 
                return null;

            if (!string.IsNullOrWhiteSpace(updateUserDto.FirstName))
                user.FirstName = updateUserDto.FirstName;

            if (!string.IsNullOrWhiteSpace(updateUserDto.LastName))
                user.LastName = updateUserDto.LastName;

            if (!string.IsNullOrWhiteSpace(updateUserDto.Country))
                user.Country = updateUserDto.Country;

            if (!string.IsNullOrWhiteSpace(updateUserDto.City))
                user.City = updateUserDto.City;

            if (!string.IsNullOrWhiteSpace(updateUserDto.Email))
            {
                bool emailExists = await _context.Users
                    .AnyAsync(u => u.Email == updateUserDto.Email && u.Id != userId && !u.IsDeleted);
                if (emailExists)
                {
                    return "Email is already in use.";
                }

                user.Email = updateUserDto.Email;
            }
                

            if (!string.IsNullOrWhiteSpace(updateUserDto.Password))
            {
                if (updateUserDto.Password != updateUserDto.RepeatPassword)
                {
                    return "Passwords do not match.";
                }

                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(updateUserDto.Password);
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return "Profile updated successfully.";
        }

        /// <summary>
        /// Marks a user as deleted (soft delete).
        /// </summary>
        /// <param name="userId">Unique identifier of the user.</param>
        /// <returns>Returns confirmation message or null if user not found.</returns>
        public async Task<string?> DeleteUserAsync(Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null || user.IsDeleted)
            {
                return null;
            }

            user.IsDeleted = true;
            await _context.SaveChangesAsync();

            return "User deleted successfully.";
        }

        /// <summary>
        /// Marks the onboarding process as completed for a user.
        /// </summary>
        /// <param name="userId">Unique identifier of the user.</param>
        /// <returns>Returns true if successful, false if user not found.</returns>
        public async Task<bool> MarkOnboardingCompletedAsync(Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId && !u.IsDeleted);
            if (user == null)
            {
                return false;
            }

            user.IsOnboardingCompleted = true;
            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Retrieves whether a user's onboarding process is completed.
        /// </summary>
        /// <param name="userId">Unique identifier of the user.</param>
        /// <returns>Returns true/false for completion status, or null if user not found.</returns>
        public async Task<bool?> GetOnboardingStatusAsync(Guid userId)
        {
            var user = await _context.Users.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId && !u.IsDeleted);
            if (user == null)
            {
                return null;
            }

            return user.IsOnboardingCompleted;
        }
    }
}
