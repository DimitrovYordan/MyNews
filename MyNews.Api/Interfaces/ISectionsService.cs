using MyNews.Api.DTOs;

namespace MyNews.Api.Interfaces
{
    public interface ISectionsService
    {
        Task<List<SectionDto>> GetAllSections();
    }
}
