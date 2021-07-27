using EPlast.BLL.DTO.Club;
using EPlast.DataAccess.Entities;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.Club
{
    public interface IClubParticipantsService
    {
        /// <summary>
        /// Get an information about a specific administrator
        /// </summary>
        /// <param name="ClubId"></param>
        /// <returns>An information about a specific administrator</returns>
        Task<IEnumerable<ClubAdministrationDTO>> GetAdministrationByIdAsync(int ClubId);

        /// <summary>
        /// Add a new administrator to the Club
        /// </summary>
        /// <param name="adminDTO">An information about a new administrator</param>
        /// <returns>An information about a specific administrator</returns>
        Task<ClubAdministrationDTO> AddAdministratorAsync(ClubAdministrationDTO adminDTO);

        /// <summary>
        /// Edit an information about a specific admininstrator
        /// </summary>
        /// <param name="adminDTO">An information about an edited administrator</param>
        /// <returns>An information about a specific admininstrator</returns>
        Task<ClubAdministrationDTO> EditAdministratorAsync(ClubAdministrationDTO adminDTO);

        /// <summary>
        /// Remove a specific administrator from the Club
        /// </summary>
        /// <param name="adminId">The id of the administrator</param>
        Task RemoveAdministratorAsync(int adminId);

        /// <summary>
        /// Removes roles from previous administrators
        /// </summary>
        Task ContinueAdminsDueToDate();

        /// <summary>
        ///returns administrations of given user
        /// </summary>
        Task<IEnumerable<ClubAdministrationDTO>> GetAdministrationsOfUserAsync(string UserId);

        /// <summary>
        ///returns administrations of given user
        /// </summary>
        Task<IEnumerable<ClubAdministrationDTO>> GetPreviousAdministrationsOfUserAsync(string UserId);
        Task<IEnumerable<ClubAdministrationStatusDTO>> GetAdministrationStatuses(string UserId);

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
        /// See <see cref="IClubParticipantsService.AddFollowerAsync(int, ClaimsPrincipal)"/> to add current user
        Task<ClubMembersDTO> AddFollowerAsync(int ClubId, string userId);

        /// <summary>
        /// Add follower to a specific Club
        /// </summary>
        /// <param name="ClubId">The id of the Club</param>
        /// <param name="user">Current user</param>
        /// <returns>An information about a new follower</returns>
        /// See <see cref="IClubParticipantsService.AddFollowerAsync(int, string)"/> to add user by id
        Task<ClubMembersDTO> AddFollowerAsync(int ClubId, User user);

        /// <summary>
        /// Toggle approve status of a specific member
        /// </summary>
        /// <param name="memberId">The id of the member</param>
        /// <returns>An information about a specific member</returns>
        Task<ClubMembersDTO> ToggleApproveStatusAsync(int memberId);

        /// <summary>
        /// Club name only for approved member
        /// </summary>
        /// <param name="memberId">The id of the member</param>
        /// <returns>club name string</returns>
        Task<string> ClubOfApprovedMember(string memberId);

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

        /// <summary>
        /// Add  follower from the Club to history table
        /// </summary>
        Task AddFollowerInHistoryAsync(int ClubId, string userId);


        /// <summary>
        /// update user in historical table insert property 
        /// </summary>
        Task UpdateStatusFollowerInHistoryAsync(string userId, bool IsFollower, bool IsDeleted);
    }
}