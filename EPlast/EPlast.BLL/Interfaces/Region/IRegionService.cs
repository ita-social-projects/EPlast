using EPlast.BLL.DTO.Admin;
using EPlast.BLL.DTO.City;
using EPlast.BLL.DTO.Region;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.DataAccess.Entities;
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
        Task<IEnumerable<CityDTO>> GetMembersAsync(int regionId);
        Task AddRegionAsync(RegionDTO region);
        Task EditRegionAsync(int regId, RegionDTO region);
        Task<RegionDTO> GetRegionByNameAsync(string Name);
        Task<IEnumerable<RegionAdministrationDTO>> GetAdministrationAsync(int regionId);
        Task<IEnumerable<AdminTypeDTO>> GetAdminTypes();
        Task AddRegionAdministrator(RegionAdministrationDTO regionAdministrationDTO);
        Task DeleteAdminByIdAsync(int Id);
        Task<IEnumerable<RegionAdministrationDTO>> GetUsersAdministrations(string userId);
        Task<IEnumerable<RegionAdministrationDTO>> GetUsersPreviousAdministrations(string userId);
        Task<RegionDocumentDTO> AddDocumentAsync(RegionDocumentDTO documentDTO);
        Task<IEnumerable<RegionDocumentDTO>> GetRegionDocsAsync(int regionId);
        Task<string> DownloadFileAsync(string fileName);
        Task DeleteFileAsync(int documentId);

        Task EndAdminsDueToDate();
        Task<string> GetLogoBase64(string logoName);
        Task<RegionAdministrationDTO> GetHead(int regionId);
        Task RedirectMembers(int prevRegId, int nextRegId);
    }
}
