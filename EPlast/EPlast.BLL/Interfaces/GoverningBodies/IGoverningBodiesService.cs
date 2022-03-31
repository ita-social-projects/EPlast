using System;
using EPlast.BLL.DTO;
using EPlast.BLL.DTO.GoverningBody;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.GoverningBodies
{
    public interface IGoverningBodiesService
    {
        Task<IEnumerable<GoverningBodyDTO>> GetGoverningBodiesListAsync();

        Task<IEnumerable<GoverningBodyDTO>> GetSectorsListAsync(int governingBodyId);

        Task<int> CreateAsync(GoverningBodyDTO governingBodyDto);

        Task<string> GetLogoBase64Async(string logoName);

        Task<GoverningBodyProfileDTO> GetGoverningBodyProfileAsync(int governingBodyId);

        Task<int> RemoveAsync(int governingBodyId);

        Task<int> EditAsync(GoverningBodyDTO governingBody);

        Task<GoverningBodyProfileDTO> GetGoverningBodyDocumentsAsync(int governingBodyId);

        Task<Dictionary<string, bool>> GetUserAccessAsync(string userId);

        Task<IEnumerable<GoverningBodyAdministrationDTO>> GetAdministrationsOfUserAsync(string UserId);

        Task<IEnumerable<GoverningBodyAdministrationDTO>> GetPreviousAdministrationsOfUserAsync(string UserId);

        /// <summary>
        /// Gets GoverningBodyAdministration history for a specific user
        /// </summary>
        /// <param name="userId">The Id of target user</param>
        /// <param name="isActive">Active status option</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>GoverningBodyAdministration history</returns>
        Task<Tuple<IEnumerable<GoverningBodyAdministrationDTO>, int>> GetAdministrationForTableAsync(
            string userId, bool isActive, int pageNumber, int pageSize);

        /// <summary>
        /// Changes status of GoverningBody admins when the date expires
        /// Adds one year to the current EndDate if the requirements are met
        /// </summary>
        Task ContinueGoverningBodyAdminsDueToDateAsync();
    }
}
