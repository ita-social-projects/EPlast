using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.BLL.DTO.AboutBase;

namespace EPlast.BLL.Interfaces.AboutBase
{
    public interface IAboutBaseSectionService
    {
        Task<IEnumerable<SectionDTO>> GetAllSectionAsync();

        Task<SectionDTO> GetSection(int id);

        Task AddSection(SectionDTO sectionDTO);

        Task ChangeSection(SectionDTO sectionDTO);

        Task DeleteSection(int id);

    }
}
