using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.BLL.DTO.AboutBase;
using EPlast.DataAccess.Entities;

namespace EPlast.BLL.Interfaces.AboutBase
{
    public interface IAboutBaseSubsectionService
    {
        Task<IEnumerable<SubsectionDto>> GetAllSubsectionAsync();

        Task<SubsectionDto> GetSubsection(int id);

        Task AddSubsection(SubsectionDto subsectionDTO, User user);

        Task ChangeSubsection(SubsectionDto subsectionDTO, User user);

        Task DeleteSubsection(int id, User user);
    }
}
