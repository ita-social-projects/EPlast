using EPlast.BLL.DTO.Region;
using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.DataAccess.Entities;

namespace EPlast.BLL.Interfaces.Region
{
    public interface IRegionAccessService
    {
        Task<IEnumerable<RegionDTO>> GetRegionsAsync(User claimsPrincipal);
        Task<bool> HasAccessAsync(User claimsPrincipal, int regionId);
        Task<IEnumerable<RegionForAdministrationDTO>> GetAllRegionsIdAndName(User user);
    }
}
