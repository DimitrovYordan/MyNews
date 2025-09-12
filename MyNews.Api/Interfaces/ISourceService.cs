using MyNews.Api.Models;

namespace MyNews.Api.Interfaces
{
    public interface ISourceService
    {
        Task<IEnumerable<Source>> GetAllAsync();

        Task<Source?> GetByIdAsync(int id);

        Task<Source> CreateAsync(Source source);

        Task<bool> UpdateAsync(int id, Source source);

        Task<bool> DeleteAsync(int id);
    }
}
