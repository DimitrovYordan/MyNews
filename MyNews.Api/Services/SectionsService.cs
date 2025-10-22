using MyNews.Api.DTOs;
using MyNews.Api.Enums;
using MyNews.Api.Interfaces;

namespace MyNews.Api.Services
{
    public class SectionsService : ISectionsService
    {
        public Task<List<SectionDto>> GetAllSections()
        {
            var result = Enum.GetValues(typeof(SectionType))
                .Cast<SectionType>()
                .Select(s => new SectionDto { Id = (int)s, Name = s.ToString() })
                .ToList();

            return Task.FromResult(result);
        }
    }
}
