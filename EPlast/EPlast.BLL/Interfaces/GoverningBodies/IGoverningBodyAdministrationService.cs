using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.BLL.DTO.GoverningBody;
using EPlast.BLL.DTO.UserProfiles;

namespace EPlast.BLL.Interfaces.GoverningBodies
{
    public interface IGoverningBodyAdministrationService
    {
        Task<Tuple<IEnumerable<GoverningBodyAdministrationDto>, int>> GetGoverningBodyAdministratorsByPageAsync(int pageNumber, int pageSize);

        Task<IEnumerable<GoverningBodyAdministrationDto>> GetGoverningBodyAdministratorsAsync();

        Task<IEnumerable<ShortUserInformationDto>> GetUsersForGoverningBodyAdminFormAsync();

        /// <summary>
        /// Adds Main Administrator
        /// </summary>
        /// <param name="governingBodyAdministrationDto">Governing Body Administration object</param>
        /// <returns>Governing Body Administration object</returns>
        Task<GoverningBodyAdministrationDto> AddGoverningBodyMainAdminAsync(GoverningBodyAdministrationDto governingBodyAdministrationDto);

        /// <summary>
        /// Adds Administrator to specified Governing Body
        /// </summary>
        /// <param name="governingBodyAdministrationDto">Governing Body Administration object</param>
        /// <returns>Governing Body Administration object</returns>
        Task<GoverningBodyAdministrationDto> AddGoverningBodyAdministratorAsync(GoverningBodyAdministrationDto governingBodyAdministrationDto);

        /// <summary>
        /// Edits Administrator of specified Governing Body
        /// </summary>
        /// <param name="governingBodyAdministrationDto">Governing Body Administration object</param>
        /// <returns>Governing Body Administration object</returns>
        Task<GoverningBodyAdministrationDto> EditGoverningBodyAdministratorAsync(GoverningBodyAdministrationDto governingBodyAdministrationDto);

        /// <summary>
        /// Removes Administrator of specified Governing Body
        /// </summary>
        /// <param name="adminId">Governing Body Administration object</param>
        Task RemoveAdministratorAsync(int adminId);


        /// <summary>
        /// Removes Main Administrator of Governing Bodies
        /// </summary>
        /// <param name="userId">Governing Body Administration object</param>
        Task RemoveMainAdministratorAsync(string userId);

        /// <summary>
        /// Removes Administration roles of user in Governing Body
        /// </summary>
        /// <param name="userId">The id of the user</param>
        Task RemoveAdminRolesByUserIdAsync(string userId);

        Task<bool> CheckRoleNameExistsAsync(string roleName);
    }
}
