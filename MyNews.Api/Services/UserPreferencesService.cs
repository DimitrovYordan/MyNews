using Microsoft.EntityFrameworkCore;

using MyNews.Api.Data;
using MyNews.Api.Enums;
using MyNews.Api.Interfaces;
using MyNews.Api.Models;

namespace MyNews.Api.Services
{
    public class UserPreferencesService : IUserPreferencesService
    {
        private readonly AppDbContext _context;

        public UserPreferencesService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SectionType>> GetSelectedSectionsAsync(Guid userId)
        {
            return await _context.UserSectionPreferences
                .Where(p => p.UserId == userId && p.IsSelected)
                .Select(p => p.SectionType)
                .ToListAsync();
        }

        public async Task UpdateSectionsAsync(Guid userId, List<SectionType> sectionIds)
        {
            var existing = await _context.UserSectionPreferences
                .Where(p => p.UserId == userId)
                .ToListAsync();

            _context.UserSectionPreferences.RemoveRange(existing);

            foreach (var sectionId in sectionIds)
            {
                _context.UserSectionPreferences.Add(new UserSectionPreference
                {
                    UserId = userId,
                    SectionType = sectionId,
                    IsSelected = true,
                    UpdatedAt = DateTime.UtcNow
                });
            }

            await _context.SaveChangesAsync();
        }
    }
}
