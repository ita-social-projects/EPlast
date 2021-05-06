using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.BLL.DTO.AboutBase;

namespace EPlast.BLL.Interfaces.AboutBase
{
    public interface IAboutBaseSubsectionService
    {
        Task<IEnumerable<SubsectionDTO>> GetAllSubsectionAsync();

        Task<SubsectionDTO> GetSubsection(int id);

        Task AddSubsection(SubsectionDTO subsectionDTO);

        Task ChangeSubsection(SubsectionDTO subsectionDTO);

        Task DeleteSubsection(int id);
    }
}
