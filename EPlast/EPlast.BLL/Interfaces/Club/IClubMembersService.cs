using EPlast.BLL.DTO.Club;
using EPlast.DataAccess.Entities;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.Club
{
    public interface IClubMembersService
    {
        /// <summary>
        /// Get all members by specific Club
        /// </summary>
        /// <param name="ClubId">The id of the Club</param>
        /// <returns>All members of a specific Club</returns>
        Task<IEnumerable<ClubMembersDTO>> GetMembersByClubIdAsync(int ClubId);

        /// <summary>
        /// Add follower to a specific Club
        /// </summary>
        /// <param name="ClubId">The id of the Club</param>
        /// <param name="userId">The id of the user</param>
        /// <returns>An information about a new follower</returns>
        /// See <see cref="IClubMembersService.AddFollowerAsync(int, ClaimsPrincipal)"/> to add current user
        Task<ClubMembersDTO> AddFollowerAsync(int ClubId, string userId);

        /// <summary>
        /// Add follower to a specific Club
        /// </summary>
        /// <param name="ClubId">The id of the Club</param>
        /// <param name="user">Current user</param>
        /// <returns>An information about a new follower</returns>
        /// See <see cref="IClubMembersService.AddFollowerAsync(int, string)"/> to add user by id
        Task<ClubMembersDTO> AddFollowerAsync(int ClubId, ClaimsPrincipal user);

        /// <summary>
        /// Toggle approve status of a specific member
        /// </summary>
        /// <param name="memberId">The id of the member</param>
        /// <returns>An information about a specific member</returns>
        Task<ClubMembersDTO> ToggleApproveStatusAsync(int memberId);

        /// <summary>
        /// Remove a specific follower from the Club
        /// </summary>
        /// <param name="followerId">The id of the follower</param>
        Task RemoveFollowerAsync(int followerId);


        /// <summary>
        /// Remove a specific follower from the Club
        /// </summary>
        /// <param name="member">The member of the club</param>
        Task RemoveMemberAsync(ClubMembers member);
    }
}