using EPlast.BLL.DTO.Region;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.Region
{
    public interface IRegionAccessService
    {
        Task<IEnumerable<RegionDTO>> GetRegionsAsync(ClaimsPrincipal claimsPrincipal);
        Task<bool> HasAccessAsync(ClaimsPrincipal claimsPrincipal, int regionId);
    }
}
