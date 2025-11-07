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
            return await _context.UserSectionPreference
                .Where(p => p.UserId == userId && p.IsSelected)
                .OrderBy(s => s.OrderIndex)
                .Select(p => p.SectionType)
                .ToListAsync();
        }

        public async Task UpdateSectionsAsync(Guid userId, List<SectionType> sectionIds)
        {
            var existing = await _context.UserSectionPreference
                .Where(p => p.UserId == userId)
                .ToListAsync();

            _context.UserSectionPreference.RemoveRange(existing);

            var newPreferences = sectionIds.Select(id => new UserSectionPreference
            {
                UserId = userId,
                SectionType = id,
                IsSelected = true,
                UpdatedAt = DateTime.UtcNow
            });

            await _context.UserSectionPreference.AddRangeAsync(newPreferences);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSectionsOrderAsync(Guid userId, List<int> sectionIds)
        {
            var userSections = await _context.UserSectionPreference
                .Where(us => us.UserId == userId && us.IsSelected)
                .ToListAsync();

            for (int i = 0; i < sectionIds.Count; i++)
            {
                var sectionId = sectionIds[i];
                var record = userSections.FirstOrDefault(us => (int)us.SectionType == sectionId);
                if (record != null)
                {
                    record.OrderIndex = i;
                    record.UpdatedAt = DateTime.UtcNow;
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<int>> GetSelectedSourcesAsync(Guid userId)
        {
            return await _context.UserSourcePreferences
                .Where(p => p.UserId == userId && p.IsSelected)
                .Select(p => p.SourceId)
                .ToListAsync();
        }

        public async Task UpdateSourcesAsync(Guid userId, List<int> sourceIds)
        {
            var existing = await _context.UserSourcePreferences
                .Where(p => p.UserId == userId)
                .ToListAsync();

            _context.UserSourcePreferences.RemoveRange(existing);

            var newPreferences = sourceIds.Select(id => new UserSourcePreferences
            {
                UserId = userId,
                SourceId = id,
                IsSelected = true,
                UpdatedAt = DateTime.UtcNow
            });

            await _context.UserSourcePreferences.AddRangeAsync(newPreferences);
            await _context.SaveChangesAsync();
        }
    }
}
