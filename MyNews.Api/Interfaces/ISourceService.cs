using MyNews.Api.Enums;
using MyNews.Api.Models;

namespace MyNews.Api.Interfaces
{
    public interface ISourceService
    {
        Task<List<Source>> GetAllSources();
    }
}
