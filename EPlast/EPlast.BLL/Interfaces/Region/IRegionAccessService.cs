using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.BLL.DTO.Region;
using EPlast.DataAccess.Entities;

namespace EPlast.BLL.Interfaces.Region
{
    public interface IRegionAccessService
    {
        Task<IEnumerable<RegionDto>> GetRegionsAsync(User claimsPrincipal);
        Task<bool> HasAccessAsync(User claimsPrincipal, int regionId);
        Task<IEnumerable<RegionForAdministrationDto>> GetAllRegionsIdAndName(User user);
    }
}
