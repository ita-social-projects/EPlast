using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.BLL.DTO.Admin;
using EPlast.BLL.DTO.Region;

namespace EPlast.BLL.Interfaces.RegionAdministrations
{
    public interface IRegionAdministrationService
    {
        Task<RegionAdministrationDto> AddRegionAdministrator(RegionAdministrationDto regionAdministrationDTO);

        Task<RegionAdministrationDto> EditRegionAdministrator(RegionAdministrationDto regionAdministrationDTO);

        Task DeleteAdminByIdAsync(int Id);

        Task<IEnumerable<RegionAdministrationDto>> GetUserAdministrations(string userId);

        Task<RegionAdministrationDto> GetRegionAdministrationByIdAsync(int regionAdministrationId);

        Task<IEnumerable<RegionAdministrationDto>> GetUserPreviousAdministrations(string userId);

        Task<RegionAdministrationDto> GetHead(int regionId);

        Task<RegionAdministrationDto> GetHeadDeputy(int regionId);

        Task<int> GetAdminType(string name);

        Task<IEnumerable<RegionAdministrationDto>> GetAdministrationAsync(int regionId);

        Task<IEnumerable<AdminTypeDto>> GetAllAdminTypes();

        Task RemoveAdminRolesByUserIdAsync(string userId);

        Task EditStatusAdministration(int adminId, bool status = false);
    }
}
