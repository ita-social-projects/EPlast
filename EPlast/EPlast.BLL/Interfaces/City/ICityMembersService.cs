using EPlast.BLL.DTO.City;
using EPlast.DataAccess.Entities;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.City
{
    public interface ICityMembersService
    {
        /// <summary>
        /// Get all members by specific city
        /// </summary>
        /// <param name="cityId">The id of the city</param>
        /// <returns>All members of a specific city</returns>
        Task<IEnumerable<CityMembersDTO>> GetMembersByCityIdAsync(int cityId);

        /// <summary>
        /// Add follower to a specific city
        /// </summary>
        /// <param name="cityId">The id of the city</param>
        /// <param name="userId">The id of the user</param>
        /// <returns>An information about a new follower</returns>
        /// See <see cref="ICityMembersService.AddFollowerAsync(int, ClaimsPrincipal)"/> to add current user
        Task<CityMembersDTO> AddFollowerAsync(int cityId, string userId);

        /// <summary>
        /// Add follower to a specific city
        /// </summary>
        /// <param name="cityId">The id of the city</param>
        /// <param name="user">Current user</param>
        /// <returns>An information about a new follower</returns>
        /// See <see cref="ICityMembersService.AddFollowerAsync(int, string)"/> to add user by id
        Task<CityMembersDTO> AddFollowerAsync(int cityId, User user);

        /// <summary>
        /// Toggle approve status of a specific member
        /// </summary>
        /// <param name="memberId">The id of the member</param>
        /// <returns>An information about a specific member</returns>
        Task<CityMembersDTO> ToggleApproveStatusAsync(int memberId);

        /// <summary>
        /// Remove a specific follower from the city
        /// </summary>
        /// <param name="followerId">The id of the follower</param>
        Task RemoveFollowerAsync(int followerId);

        /// <summary>
        /// Remove a specific follower from the city
        /// </summary>
        /// <param name="member">Member of the city</param>
        Task RemoveMemberAsync(CityMembers member);
    }
}