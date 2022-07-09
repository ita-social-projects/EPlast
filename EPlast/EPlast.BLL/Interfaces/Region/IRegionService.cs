using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.BLL.DTO.City;
using EPlast.BLL.DTO.Region;
using EPlast.DataAccess.Entities;
using EPlast.Resources;

namespace EPlast.BLL.Interfaces.Region
{
    public interface IRegionService
    {
        Task ArchiveRegionAsync(int regionId);
        Task UnArchiveRegionAsync(int regionId);
        Task<IEnumerable<RegionDto>> GetAllRegionsAsync();
        Task<IEnumerable<RegionDto>> GetAllActiveRegionsAsync();
        Task<IEnumerable<RegionDto>> GetAllNotActiveRegionsAsync();
        Task<RegionDto> GetRegionByIdAsync(int regionId);
        Task<RegionProfileDto> GetRegionProfileByIdAsync(int regionId, User user);
        Task DeleteRegionByIdAsync(int regionId);
        Task AddFollowerAsync(int regionId, int cityId);
        Task<IEnumerable<CityDto>> GetMembersAsync(int regionId);
        Task AddRegionAsync(RegionDto region);
        Task EditRegionAsync(int regId, RegionDto region);
        Task<Tuple<IEnumerable<RegionObjectsDto>, int>> GetAllRegionsByPageAndIsArchiveAsync(int page, int pageSize, string regionName, bool isArchive);
        Task<RegionDto> GetRegionByNameAsync(string Name);
        Task<RegionProfileDto> GetRegionByNameAsync(string Name, User user);
        Task<RegionDocumentDto> AddDocumentAsync(RegionDocumentDto documentDTO);
        Task<IEnumerable<RegionDocumentDto>> GetRegionDocsAsync(int regionId);
        Task<string> DownloadFileAsync(string fileName);
        Task DeleteFileAsync(int documentId);
        Task ContinueAdminsDueToDate();
        Task<string> GetLogoBase64(string logoName);
        Task RedirectMembers(int prevRegId, int nextRegId);

        /// <summary>
        /// Gets all Regions
        /// </summary>
        /// <returns>All Regions</returns>
        Task<IEnumerable<RegionForAdministrationDto>> GetRegions(UkraineOblasts oblast = UkraineOblasts.NotSpecified);

        /// <summary>
        /// >Gets Region Users 
        /// </summary>
        /// <returns> All users of cities included in this region</returns>
        Task<IEnumerable<RegionUserDto>> GetRegionUsersAsync(int regionId);

        /// <summary>
        /// Gets a list of followers of a specific region
        /// </summary>
        /// <param name="regionId">The id of the region</param>
        /// <returns>A list of followers of a specific region</returns>
        Task<IEnumerable<RegionFollowerDto>> GetFollowersAsync(int regionId);

        /// <summary>
        /// Gets a specific follower of the region
        /// </summary>
        /// <param name="followerId">The id of the follower</param>
        Task<RegionFollowerDto> GetFollowerAsync(int followerId);

        /// <summary>
        /// Creates a new follower
        /// </summary>
        /// <param name="model">An information about a new follower</param>
        Task<int> CreateFollowerAsync(RegionFollowerDto model);

        /// <summary>
        /// Removes a specific follower from the region
        /// </summary>
        /// <param name="followerId">The id of the follower</param>
        Task RemoveFollowerAsync(int followerId);

        /// <summary>
        /// Get Active Regions Names
        /// </summary>
        IEnumerable<RegionNamesDto> GetActiveRegionsNames();

        /// <summary>
        /// Get true if region name is exist
        /// </summary>
        /// <param name="name">The name of the region</param>
        Task<bool> CheckIfRegionNameExistsAsync(string name);
    }
}
