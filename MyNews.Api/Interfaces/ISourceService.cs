using MyNews.Api.Enums;
using MyNews.Api.Models;

namespace MyNews.Api.Interfaces
{
    public interface ISourceService
    {
        Task<IEnumerable<Source>> GetBySectionAsync(SectionType section);

        Task<Source?> GetByIdAsync(int id);

        Task<Source?> GetByNameAsync(string name);

        Task<Source> CreateAsync(Source source);

        Task<bool> UpdateAsync(int id, Source source);

        Task<bool> DeleteAsync(int id);
    }
}
