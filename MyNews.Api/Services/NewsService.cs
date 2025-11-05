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

        public async Task<List<NewsItemDto>> GetNewsBySectionsAndSourcesAsync(IEnumerable<int> sectionIds, IEnumerable<int> sourceIds, Guid userId)
        {
            var query = _context.NewsItems
                .Include(n => n.Source)
                .Include(n => n.Translations)
                .AsQueryable();

            if (sectionIds != null && sectionIds.Any())
                query = query.Where(n => sectionIds.Contains((int)n.Section));

            if (sourceIds != null && sourceIds.Any())
                query = query.Where(n => sourceIds.Contains(n.SourceId));

            var news = await query
                .OrderByDescending(n => n.PublishedAt)
                .ToListAsync();

            var readNewsIds = await _context.UserNewsReads
                .Where(r => r.UserId == userId)
                .Select(r => r.NewsItemId)
                .ToListAsync();

            return news.Select(n => new NewsItemDto
            {
                Id = n.Id,
                Section = n.Section,
                Title = n.Title,
                Summary = n.Summary ?? string.Empty,
                PublishedAt = n.PublishedAt,
                Link = n.Link,
                SourceName = n.Source?.Name ?? string.Empty,
                SourceUrl = n.Source?.Url ?? string.Empty,
                IsNew = !readNewsIds.Contains(n.Id),
                IsRead = readNewsIds.Contains(n.Id),
                Translations = n.Translations?.Select(t => new NewsTranslationDto
                {
                    LanguageCode = t.LanguageCode,
                    Title = t.Title,
                    Summary = t.Summary
                }).ToList()
            }).ToList();
        }

        public async Task<IEnumerable<SectionWithNewsDto>> GetNewsBySectionsAsync(List<SectionType> sectionIds, Guid userId)
        {
            var selectedSections = sectionIds.Any() 
                ? sectionIds 
                : Enum.GetValues(typeof(SectionType)).Cast<SectionType>().ToList();

            var newsItems = await _context.NewsItems
                .Include(n => n.Source)
                .Include(n => n.Translations)
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

                        Translations = n.Translations.Select(t => new NewsTranslationDto
                        {
                            LanguageCode = t.LanguageCode,
                            Title = t.Title,
                            Summary = t.Summary,
                        }).ToList()
                    }).ToList()
                })
                .ToList();

            return result;
        }

        public async Task MarkAsReadAsync(Guid userId, Guid newsItemId)
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
                    HasClickedLink = false
                };

                _context.UserNewsReads.Add(read);
            }
            else
            {
                read.HasClickedTitle = true;
                read.ReadAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
        }

        public async Task MarkLinkClickedAsync(Guid userId, Guid newsItemId)
        {
            var read = await _context.UserNewsReads
                .FirstOrDefaultAsync(r => r.UserId == userId && r.NewsItemId == newsItemId);

            if (read != null)
            {
                read.HasClickedLink = true;
                read.ReadAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
    }
}
