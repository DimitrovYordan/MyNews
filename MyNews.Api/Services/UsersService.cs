using Microsoft.EntityFrameworkCore;

using MyNews.Api.Data;
using MyNews.Api.DTOs;
using MyNews.Api.Interfaces;

namespace MyNews.Api.Services
{
    public class UsersService : IUserService
    {
        public readonly AppDbContext _context;

        public UsersService(AppDbContext context)
        {
            _context = context;
        }

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
