using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.BLL.DTO;
using EPlast.BLL.DTO.GoverningBody.Sector;

namespace EPlast.BLL.Interfaces.GoverningBodies.Sector
{
    public interface ISectorService
    {
        Task<int> CreateAsync(SectorDTO sectorDto);

        Task<IEnumerable<SectorDTO>> GetSectorsByGoverningBodyAsync(int governingBodyId);

        Task<string> GetLogoBase64Async(string logoName);

        Task<SectorProfileDTO> GetSectorProfileAsync(int sectorId);

        Task<Dictionary<string, bool>> GetUserAccessAsync(string userId);
        
        Task<SectorProfileDTO> GetSectorDocumentsAsync(int sectorId);

        Task<int> EditAsync(SectorDTO sector);

        Task<int> RemoveAsync(int sectorId);
    }
}
