using Microsoft.EntityFrameworkCore;

using MyNews.Api.Data;
using MyNews.Api.Enums;
using MyNews.Api.Interfaces;
using MyNews.Api.Models;

namespace MyNews.Api.Services
{
    public class SourcesService : ISourceService
    {
        private readonly AppDbContext _context;

        public SourcesService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Source?> GetByIdAsync(int id)
        {
            return await _context.Sources.FindAsync(id);
        }

        public async Task<Source> CreateAsync(Source source)
        {
            _context.Sources.Add(source);
            await _context.SaveChangesAsync();

            return source;
        }

        public async Task<bool> UpdateAsync(int id, Source source)
        {
            if (id != source.Id)
            {
                return false;
            }

            _context.Entry(source).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var source = await _context.Sources.FindAsync(id);
            if (source == null)
            {
                return false;
            }

            _context.Sources.Remove(source);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<Source>> GetBySectionAsync(SectionType section)
        {
            return await _context.Sources
                .Where(s => s.Section == section)
                .Include(n => n.NewsItems)
                .ToListAsync();
        }
    }
}
