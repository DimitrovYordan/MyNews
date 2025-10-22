using Microsoft.EntityFrameworkCore;

using MyNews.Api.Data;
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

        public async Task<List<Source>> GetAllSources()
        {
            return await _context.Sources.ToListAsync();
        }
    }
}
