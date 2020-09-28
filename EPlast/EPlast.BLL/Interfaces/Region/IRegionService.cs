using EPlast.BLL.DTO.Region;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.Region
{
    public interface IRegionService
    {
        Task<IEnumerable<RegionDTO>> GetAllRegionsAsync();
        Task<RegionDTO> GetRegionByIdAsync(int regionId);
        Task<RegionProfileDTO> GetRegionProfileByIdAsync(int regionId);
        Task DeleteRegionByIdAsync(int cityId);
        Task AddFollowerAsync(int regionId, int cityId);
    }
}
