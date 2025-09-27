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

        public async Task<IEnumerable<SectionWithNewsDto>> GetNewsBySectionsAsync(List<SectionType> sectionIds, Guid userId)
        {
            var selectedSections = sectionIds.Any() 
                ? sectionIds 
                : Enum.GetValues(typeof(SectionType)).Cast<SectionType>().ToList();

            var newsItems = await _context.NewsItems
                .Include(n => n.Source)
                .Where(n => selectedSections.Contains(n.Section))
                .OrderByDescending(n => n.PublishedAt)
                .ToListAsync();

            var readNewsIds = await _context.UserNewsReads
                .Where(r => r.UserId == userId)
                .Select(r => r.NewsItemId)
                .ToListAsync();

            var result = newsItems
                .GroupBy(n => n.Section)
                .Select(g => new SectionWithNewsDto
                {
                    SectionId = (int)g.Key,
                    SectionName = g.Key.ToString(),
                    News = g.Select(n => new NewsItemDto
                    {
                        Id = n.Id,
                        Section = n.Section,
                        Title = n.Title,
                        PublishedAt = n.PublishedAt,
                        SourceName = n.Source?.Name ?? string.Empty,
                        SourceUrl = n.Source?.Url ?? string.Empty,
                        Summary = n.Summary,
                        Link = n.Link,
                        IsNew = !readNewsIds.Contains(n.Id),
                        IsRead = readNewsIds.Contains(n.Id),
                    }).ToList()
                })
                .ToList();

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

        public async Task MarkNewsInteractionAsync(Guid userId, Guid newsItemId, bool clickedLink = false)
        {
            var read = await _context.UserNewsReads
                .FirstOrDefaultAsync(r => r.UserId == userId && r.NewsItemId == newsItemId);

            if (read == null)
            {
                read = new UserNewsRead
                {
                    UserId = userId,
                    NewsItemId = newsItemId,
                    ReadAt = DateTime.UtcNow,
                    HasClickedTitle = true,
                    HasClickedLink = clickedLink
                };
                _context.UserNewsReads.Add(read);
            }
            else
            {
                read.HasClickedTitle = true;
                if (clickedLink)
                    read.HasClickedLink = true;

                read.ReadAt = DateTime.UtcNow;
                _context.UserNewsReads.Update(read);
            }

            await _context.SaveChangesAsync();
        }
    }
}
