using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.BLL.DTO.AboutBase;
using EPlast.DataAccess.Entities;


namespace EPlast.BLL.Interfaces.AboutBase
{
    public interface IAboutBaseSectionService
    {
        Task<IEnumerable<SectionDto>> GetAllSectionAsync();

        Task<SectionDto> GetSection(int id);

        Task AddSection(SectionDto sectionDTO, User user);

        Task ChangeSection(SectionDto sectionDTO, User user);

        Task DeleteSection(int id, User user);

    }
}
