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
                var newsItems = await _context.NewsItems
                    .Include(n => n.Source)
                    .Where(n => n.Section == section)
                    .OrderByDescending(n => n.PublishedAt)
                    .ToListAsync();

                var newsDtos = new List<NewsItemDto>();

                foreach (var n in newsItems)
                {
                    newsDtos.Add(new NewsItemDto
                    {
                        Title = n.Title,
                        PublishedAt = n.PublishedAt,
                        SourceName = n.Source?.Name ?? string.Empty,
                        SourceUrl = n.Source?.Url ?? string.Empty,
                        Summary = string.Empty,
                        Link = string.Empty,
                        IsNew = false
                    });
                }

                result.Add(new SectionWithNewsDto
                {
                    SectionId = (int)section,
                    SectionName = section.ToString(),
                    News = newsDtos
                });
            }


            return result;
        }

        public async Task<bool> ExistsByTitleAndSourceAsync(string title, int sourceId)
        {
            return await _context.NewsItems.AnyAsync(n => n.Title == title && n.SourceId == sourceId);
        }

        public async Task<NewsItem> AddNewsItemAsync(NewsItem newsItem)
        {
            _context.NewsItems.Add(newsItem);
            await _context.SaveChangesAsync();

            return newsItem;
        }

        public async Task MarkAsReadAsync(int userId, Guid newsItemId)
        {
            bool alreadyRead = await _context.UserNewsReads
                .AnyAsync(r => r.UserId == userId && r.NewsItemId == newsItemId);

            if (!alreadyRead)
            {
                _context.UserNewsReads.Add(new UserNewsRead
                {
                    UserId = userId,
                    NewsItemId = newsItemId,
                    ReadAt = DateTime.UtcNow
                });

                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> IsReadAsync(int userId, Guid newsItemId)
        {
            return await _context.UserNewsReads.AnyAsync(r => r.UserId == userId && r.NewsItemId == newsItemId);
        }
    }
}
