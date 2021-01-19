using EPlast.BLL.DTO;
using EPlast.BLL.DTO.City;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.Interfaces
{
    public interface IAdminService
    {
        /// <summary>
        /// Get all roles except Admin role
        /// </summary>
        /// <returns>All roles except Admin role</returns>
        Task<IEnumerable<IdentityRole>> GetRolesExceptAdminAsync();

        /// <summary>
        /// Edit user roles
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <param name="roles">List of new user roles</param>
        Task EditAsync(string userId, List<string> roles);

        /// <summary>
        /// Edit user roles
        /// </summary>
        /// <param name="userId">The id of the user</param>
        Task ChangeAsync(string userId);

        /// <summary>
        /// Delete a user
        /// </summary>
        /// <param name="userId">The id of the user, which must be deleted</param>
        Task DeleteUserAsync(string userId);

        /// <summary>
        /// Get all users with additional information
        /// </summary>
        /// <returns>Specify model with all users</returns>
        Task<IEnumerable<UserTableDTO>> UsersTableAsync();

        /// <summary>
        /// Get all users with role "Зацікавлений"
        /// </summary>
        /// <returns>Specify model with all users</returns>
        Task<IEnumerable<UserTableDTO>> InterestedUsersTableAsync();


        /// <summary>
        /// Get all users with no confirmed email
        /// </summary>
        /// <returns>Specify model with all users</returns>
        Task<IEnumerable<UserTableDTO>> InactiveUsersTableAsync();

        /// <summary>
        /// Change Current Role of user
        /// </summary>
        Task ChangeCurrentRoleAsync(string userId, string role);

        /// <summary>
        /// Get City and Region Admins by userId of user which contained cityMembers
        /// </summary>
        Task<IEnumerable<CityDTO>> GetCityRegionAdminsOfUser(string userId);
    }
}
