using EPlast.BLL.DTO.Admin;
using EPlast.BLL.DTO.City;
using EPlast.BLL.DTO.Region;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.DataAccess.Entities;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.Region
{
    public interface IRegionService
    {
        Task<IEnumerable<RegionDTO>> GetAllRegionsAsync();
        Task<RegionDTO> GetRegionByIdAsync(int regionId);
        Task<RegionProfileDTO> GetRegionProfileByIdAsync(int regionId, User user);
        Task DeleteRegionByIdAsync(int regionId);
        Task AddFollowerAsync(int regionId, int cityId);
        Task<IEnumerable<CityDTO>> GetMembersAsync(int regionId);
        Task AddRegionAsync(RegionDTO region);
        Task EditRegionAsync(int regId, RegionDTO region);
        Task<RegionDTO> GetRegionByNameAsync(string Name);
        Task<RegionProfileDTO> GetRegionByNameAsync(string Name, User user);
        Task<RegionDocumentDTO> AddDocumentAsync(RegionDocumentDTO documentDTO);
        Task<IEnumerable<RegionDocumentDTO>> GetRegionDocsAsync(int regionId);
        Task<string> DownloadFileAsync(string fileName);
        Task DeleteFileAsync(int documentId);
        Task ContinueAdminsDueToDate();
        Task<string> GetLogoBase64(string logoName);
        Task RedirectMembers(int prevRegId, int nextRegId);

        /// <summary>
        /// Get all Regions
        /// </summary>
        /// <returns>All Regions</returns>
        Task<IEnumerable<RegionForAdministrationDTO>> GetRegions();

        /// <summary>
        /// >Get Region Users 
        /// </summary>
        /// <returns> All users of cities included in this region</returns>
        Task<IEnumerable<RegionUserDTO>> GetRegionUsersAsync(int regionId);
    }
}
