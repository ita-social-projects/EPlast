using System.Threading.Tasks;
using EPlast.BLL.DTO.GoverningBody.Sector;

namespace EPlast.BLL.Interfaces.GoverningBodies.Sector
{
    public interface ISectorAdministrationService
    {
        Task<SectorAdministrationDTO> AddSectorAdministratorAsync(SectorAdministrationDTO sectorAdministrationDto);

        Task<SectorAdministrationDTO> EditSectorAdministratorAsync(SectorAdministrationDTO sectorAdministrationDto);

        Task RemoveAdministratorAsync(int adminId);
    }
}
