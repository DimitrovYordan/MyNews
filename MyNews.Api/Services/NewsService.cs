using Microsoft.EntityFrameworkCore;

using MyNews.Api.Data;
using MyNews.Api.DTOs;
using MyNews.Api.Enums;
using MyNews.Api.Interfaces;
using MyNews.Api.Models;

namespace MyNews.Api.Services
{
    public class NewsService : INewsService
    {
        private readonly AppDbContext _context;

        public NewsService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<NewsItem>> GetNewsAsync(List<int> sectionsIds)
        {
            return await _context.NewsItems
                .Where(n => sectionsIds.Count == 0 || sectionsIds.Contains((int)n.Section))
                .OrderByDescending(n => n.PublishedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<SectionWithNewsDto>> GetNewsBySectionsAsync(List<SectionType> sectionIds)
        {
            var selectedSections = sectionIds.Any() ? sectionIds : Enum.GetValues(typeof(SectionType)).Cast<SectionType>().ToList();

            var result = new List<SectionWithNewsDto>();

            foreach (var section in selectedSections)
            {
                var news = await _context.NewsItems
                    .Include(n => n.Source)
                    .Where(n => n.Section == section)
                    .OrderByDescending(n => n.PublishedAt)
                    .Select(n => new NewsItemDto
                    {
                        Id = n.Id,
                        Title = n.Title,
                        Content = n.Content,
                        PublishedAt = n.PublishedAt,
                        SourceName = n.Source != null ? n.Source.Name : string.Empty,
                        SourceUrl = n.Source != null ? n.Source.Url : string.Empty
                    })
                    .ToListAsync();

                result.Add(new SectionWithNewsDto
                {
                    SectionId = (int)section,
                    SectionName = section.ToString(),
                    News = news
                });
            }

            return result;
        }
    }
}
