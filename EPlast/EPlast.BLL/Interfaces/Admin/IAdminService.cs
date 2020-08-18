using EPlast.BLL.DTO;
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
    }
}
