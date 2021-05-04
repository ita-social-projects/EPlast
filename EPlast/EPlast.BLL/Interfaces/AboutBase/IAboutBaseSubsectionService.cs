using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.BLL.DTO.AboutBase;
using EPlast.DataAccess.Entities;

namespace EPlast.BLL.Interfaces.AboutBase
{
    interface IAboutBaseSubsectionService
    {
        Task<IEnumerable<SubsectionDTO>> GetAllSubsectionAsync();

        Task<SubsectionDTO> GetSubsection(int id);

        Task AddSubsection(SubsectionDTO subsectionDTO, User user);

        Task ChangeSubsection(SubsectionDTO subsectionDTO, User user);

        Task DeleteSubsection(int id, User user);
    }
}
