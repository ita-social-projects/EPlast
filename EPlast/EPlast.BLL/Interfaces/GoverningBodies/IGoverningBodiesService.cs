using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.BLL.DTO;
using EPlast.BLL.DTO.GoverningBody;

namespace EPlast.BLL.Interfaces.GoverningBodies
{
    public interface IGoverningBodiesService
    {
        Task<IEnumerable<GoverningBodyDto>> GetGoverningBodiesListAsync();

        Task<IEnumerable<GoverningBodyDto>> GetSectorsListAsync(int governingBodyId);

        Task<int> CreateAsync(GoverningBodyDto governingBodyDto);

        Task<string> GetLogoBase64Async(string logoName);

        Task<GoverningBodyProfileDto> GetGoverningBodyProfileAsync(int governingBodyId);

        Task<int> RemoveAsync(int governingBodyId);

        Task<int> EditAsync(GoverningBodyDto governingBody);

        Task<GoverningBodyProfileDto> GetGoverningBodyDocumentsAsync(int governingBodyId);

        Task<Dictionary<string, bool>> GetUserAccessAsync(string userId);

        Task<IEnumerable<GoverningBodyAdministrationDto>> GetAdministrationsOfUserAsync(string UserId);

        Task<IEnumerable<GoverningBodyAdministrationDto>> GetPreviousAdministrationsOfUserAsync(string UserId);

        /// <summary>
        /// Gets GoverningBodyAdministration history for a specific user
        /// </summary>
        /// <param name="userId">The Id of target user</param>
        /// <param name="isActive">Active status option</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>GoverningBodyAdministration history</returns>
        Task<Tuple<IEnumerable<GoverningBodyAdministrationDto>, int>> GetAdministrationForTableAsync(
            string userId, bool isActive, int pageNumber, int pageSize);

        /// <summary>
        /// Changes status of GoverningBody admins when the date expires
        /// Adds one year to the current EndDate if the requirements are met
        /// </summary>
        Task ContinueGoverningBodyAdminsDueToDateAsync();
    }
}
