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
            return await _context.UserPreferences
                .Where(p => p.UserId == userId && p.IsSelected && p.SectionType.HasValue)
                .Select(p => p.SectionType.Value)
                .ToListAsync();
        }

        public async Task UpdateSectionsAsync(Guid userId, List<SectionType> sectionIds)
        {
            var existing = await _context.UserPreferences
                .Where(p => p.UserId == userId)
                .ToListAsync();

            _context.UserPreferences.RemoveRange(existing);

            foreach (var sectionId in sectionIds)
            {
                _context.UserPreferences.Add(new UserPreferences
                {
                    UserId = userId,
                    SectionType = sectionId,
                    IsSelected = true,
                    UpdatedAt = DateTime.UtcNow
                });
            }

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<int>> GetSelectedSourcesAsync(Guid userId)
        {
            return await _context.UserPreferences
                .Where(p => p.UserId == userId && p.IsSelected && p.SourceId != null)
                .Select(p => p.SourceId!.Value)
                .ToListAsync();
        }

        public async Task UpdateSourcesAsync(Guid userId, List<int> sourceIds)
        {
            var existing = await _context.UserPreferences
                .Where(p => p.UserId == userId && p.SourceId != null)
                .ToListAsync();

            _context.UserPreferences.RemoveRange(existing);

            foreach (var sourceId in sourceIds)
            {
                _context.UserPreferences.Add(new UserPreferences
                {
                    UserId = userId,
                    SectionType = null,
                    SourceId = sourceId,
                    IsSelected = true,
                    UpdatedAt = DateTime.UtcNow
                });
            }

            await _context.SaveChangesAsync();
        }
    }
}
