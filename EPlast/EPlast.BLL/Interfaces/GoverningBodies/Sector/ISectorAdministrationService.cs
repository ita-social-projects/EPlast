using System.Threading.Tasks;
using EPlast.BLL.DTO.GoverningBody.Sector;

namespace EPlast.BLL.Interfaces.GoverningBodies.Sector
{
    public interface ISectorAdministrationService
    {
        Task<SectorAdministrationDto> AddSectorAdministratorAsync(SectorAdministrationDto sectorAdministrationDto);

        Task<SectorAdministrationDto> EditSectorAdministratorAsync(SectorAdministrationDto sectorAdministrationDto);

        Task RemoveAdministratorAsync(int adminId);

        Task RemoveAdminRolesByUserIdAsync(string userId);
    }
}
