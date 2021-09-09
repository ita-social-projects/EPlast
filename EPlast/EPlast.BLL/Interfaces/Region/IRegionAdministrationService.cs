using EPlast.BLL.DTO.Admin;
using EPlast.BLL.DTO.Region;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.Region
{
    public interface IRegionAdministrationService
    {
        Task<RegionAdministrationDTO> AddRegionAdministrator(RegionAdministrationDTO regionAdministrationDTO);

        Task<RegionAdministrationDTO> EditRegionAdministrator(RegionAdministrationDTO regionAdministrationDTO);

        Task DeleteAdminByIdAsync(int Id);

        Task<IEnumerable<RegionAdministrationDTO>> GetUsersAdministrations(string userId);

        Task<IEnumerable<RegionAdministrationDTO>> GetUsersPreviousAdministrations(string userId);

        Task<RegionAdministrationDTO> GetHead(int regionId);
        Task<RegionAdministrationDTO> GetHeadDeputy(int regionId);

        Task<int> GetAdminType(string name);

        Task<IEnumerable<RegionAdministrationDTO>> GetAdministrationAsync(int regionId);

        Task<IEnumerable<AdminTypeDTO>> GetAllAdminTypes();
    }
}
