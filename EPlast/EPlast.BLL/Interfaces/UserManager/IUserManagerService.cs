using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.DataAccess.Entities;

namespace EPlast.BLL.Services.Interfaces
{
    public interface IUserManagerService
    {
        /// <summary>
        /// Checks whether target user is in listed roles or not
        /// </summary>
        /// <param name="user">The user(dto) whose roles you want to check</param>
        /// <param name="roles">The roles to check</param>
        /// <returns>Is this user in listed roles</returns>
        Task<bool> IsInRoleAsync(UserDTO user, params string[] roles);

        /// <summary>
        /// Finds user by Id
        /// </summary>
        /// <param name="userId">The Id of user you want to find</param>
        /// <returns>User(dto) by the Id you requested</returns>
        Task<UserDTO> FindByIdAsync(string userId);

        /// <summary>
        /// Gets list of roles for requested user
        /// </summary>
        /// <param name="user">The user (dto) you want to get the roles for</param>
        /// <returns>List of roles for the requested user</returns>
        Task<IEnumerable<string>> GetRolesAsync(UserDTO user);

        /// <summary>
        /// Gets user by the claim-based identity
        /// </summary>
        /// <param name="principal">The claim-based user identity</param>
        /// <returns>User by the identity</returns>
        Task<User> GetCurrentUserAsync(ClaimsPrincipal principal);

        /// <summary>
        /// Gets the Id of user by the claim-based identity
        /// </summary>
        /// <param name="principal">The claim-based user identity</param>
        /// <returns>The Id of user by the identity</returns>
        string GetCurrentUserId(ClaimsPrincipal principal);

        /// <summary>
        /// Gets list of roles for requested user
        /// </summary>
        /// <param name="user">The user you want to get the roles for</param>
        /// <returns>List of roles for the requested user</returns>
        Task<IList<string>> GetUserRolesAsync(User user);

        /// <summary>
        /// Finds user by Id
        /// </summary>
        /// <param name="userId">The Id of user you want to find</param>
        /// <returns>User by the Id you requested</returns>
        Task<User> FindUserByIdAsync(string userId);

        /// <summary>
        /// Finds user by email
        /// </summary>
        /// <param name="email">The email of user you want to find</param>
        /// <returns>User by the email you requested</returns>
        Task<User> FindUserByEmailAsync(string email);

        /// <summary>
        /// Checks whether target user is in specified role or not
        /// </summary>
        /// <param name="user">The user(dto) whose roles you want to check</param>
        /// <param name="role">The roles to check</param>
        /// <returns>Is this user in listed roles</returns>
        Task<bool> IsUserInRoleAsync(User user, string role);
    }
}
