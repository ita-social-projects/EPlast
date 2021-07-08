using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.BLL.DTO.AboutBase;
using EPlast.DataAccess.Entities;


namespace EPlast.BLL.Interfaces.AboutBase
{
    public interface IAboutBaseSectionService
    {
        Task<IEnumerable<SectionDTO>> GetAllSectionAsync();

        Task<SectionDTO> GetSection(int id);

        Task AddSection(SectionDTO sectionDTO, User user);

        Task ChangeSection(SectionDTO sectionDTO, User user);

        Task DeleteSection(int id, User user);

    }
}
