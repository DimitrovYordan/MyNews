using MyNews.Api.Enums;
using MyNews.Api.Interfaces;

namespace MyNews.Api.Services
{
    public class SectionsService : ISectionsService
    {
        public IEnumerable<object> GetSections()
        {
            return Enum.GetValues(typeof(SectionType))
                .Cast<SectionType>()
                .Select(s => new { Id = (int)s, Name = s.ToString() })
                .ToList();
        }
    }
}
