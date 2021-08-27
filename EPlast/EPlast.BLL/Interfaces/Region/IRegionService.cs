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
        Task ArchiveRegionAsync(int regionId);
        Task UnArchiveRegionAsync(int regionId);
        Task<IEnumerable<RegionDTO>> GetAllRegionsAsync();
        Task<IEnumerable<RegionDTO>> GetAllActiveRegionsAsync();
        Task<IEnumerable<RegionDTO>> GetAllNotActiveRegionsAsync();
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
        /// Gets all Regions
        /// </summary>
        /// <returns>All Regions</returns>
        Task<IEnumerable<RegionForAdministrationDTO>> GetRegions();

        /// <summary>
        /// >Gets Region Users 
        /// </summary>
        /// <returns> All users of cities included in this region</returns>
        Task<IEnumerable<RegionUserDTO>> GetRegionUsersAsync(int regionId);

        /// <summary>
        /// Gets a list of followers of a specific region
        /// </summary>
        /// <param name="regionId">The id of the region</param>
        /// <returns>A list of followers of a specific region</returns>
        Task<IEnumerable<RegionFollowerDTO>> GetFollowersAsync(int regionId);

        /// <summary>
        /// Gets a specific follower of the region
        /// </summary>
        /// <param name="followerId">The id of the follower</param>
        Task<RegionFollowerDTO> GetFollowerAsync(int followerId);

        /// <summary>
        /// Creates a new follower
        /// </summary>
        /// <param name="model">An information about a new follower</param>
        Task CreateFollowerAsync(RegionFollowerDTO model);

        /// <summary>
        /// Removes a specific follower from the region
        /// </summary>
        /// <param name="followerId">The id of the follower</param>
        Task RemoveFollowerAsync(int followerId);
        Task<bool> CheckRegionNameExistsAsync(string name);
    }
}
