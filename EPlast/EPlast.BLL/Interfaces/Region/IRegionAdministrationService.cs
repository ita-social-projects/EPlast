using EPlast.BLL.DTO.Region;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.Region
{
    public interface IRegionAdministrationService
    {
        Task<IEnumerable<RegionAdministrationDTO>> GetAdministrationByIdAsync(int regionId);
        Task<RegionAdministrationDTO> AddAdministratorAsync(RegionAdministrationDTO adminDTO);
        Task<RegionAdministrationDTO> EditAdministratorAsync(RegionAdministrationDTO adminDTO);
        Task RemoveAdministratorAsync(int adminId);
    }
}
