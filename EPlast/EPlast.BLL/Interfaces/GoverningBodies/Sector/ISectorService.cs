using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.BLL.DTO.GoverningBody.Sector;

namespace EPlast.BLL.Interfaces.GoverningBodies.Sector
{
    public interface ISectorService
    {
        Task<int> CreateAsync(SectorDto sectorDto);

        Task<IEnumerable<SectorDto>> GetSectorsByGoverningBodyAsync(int governingBodyId);

        Task<string> GetLogoBase64Async(string logoName);

        Task<SectorProfileDto> GetSectorProfileAsync(int sectorId);

        Task<Dictionary<string, bool>> GetUserAccessAsync(string userId);

        Task<SectorProfileDto> GetSectorDocumentsAsync(int sectorId);

        Task<int> EditAsync(SectorDto sector);

        Task<int> RemoveAsync(int sectorId);

        Task<IEnumerable<SectorAdministrationDto>> GetAdministrationsOfUserAsync(string UserId);

        Task<IEnumerable<SectorAdministrationDto>> GetPreviousAdministrationsOfUserAsync(string UserId);

        /// <summary>
        /// Gets SectorAdministration history for a specific user
        /// </summary>
        /// <param name="userId">The Id of target user</param>
        /// <param name="isActive">Active status option</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>SectorAdministration history</returns>
        Task<Tuple<IEnumerable<SectorAdministrationDto>, int>> GetAdministrationForTableAsync(
            string userId, bool isActive, int pageNumber, int pageSize);

        /// <summary>
        /// Changes status of Sector admins when the date expires
        /// Adds one year to the current EndDate if the requirements are met
        /// </summary>
        Task ContinueSectorAdminsDueToDateAsync();
    }
}
