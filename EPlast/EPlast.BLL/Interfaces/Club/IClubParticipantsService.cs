using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using EPlast.BLL.DTO.Club;
using EPlast.DataAccess.Entities;

namespace EPlast.BLL.Interfaces.Club
{
    public interface IClubParticipantsService
    {
        /// <summary>
        /// Get an information about a specific administrator
        /// </summary>
        /// <param name="clubId"></param>
        /// <returns>An information about a specific administrator</returns>
        Task<IEnumerable<ClubAdministrationDto>> GetAdministrationByIdAsync(int clubId);

        /// <summary>
        /// Add a new administrator to the Club
        /// </summary>
        /// <param name="adminDTO">An information about a new administrator</param>
        /// <returns>An information about a specific administrator</returns>
        Task<ClubAdministrationDto> AddAdministratorAsync(ClubAdministrationDto adminDTO);

        /// <summary>
        /// Edit an information about a specific administrator
        /// </summary>
        /// <param name="adminDTO">An information about an edited administrator</param>
        /// <returns>An information about a specific administrator</returns>
        Task<ClubAdministrationDto> EditAdministratorAsync(ClubAdministrationDto adminDTO);

        /// <summary>
        /// Remove a specific administrator from the Club
        /// </summary>
        /// <param name="adminId">The id of the administrator</param>
        Task RemoveAdministratorAsync(int adminId);

        /// <summary>
        /// Removes Administration roles of user in the Club
        /// </summary>
        /// <param name="userId">The id of the user</param>
        Task RemoveAdminRolesByUserIdAsync(string userId);

        /// <summary>
        /// Removes roles from previous administrators
        /// </summary>
        Task ContinueAdminsDueToDate();

        /// <summary>
        ///returns administrations of given user
        /// </summary>
        Task<IEnumerable<ClubAdministrationDto>> GetAdministrationsOfUserAsync(string userId);

        /// <summary>
        ///returns administrations of given user
        /// </summary>
        Task<IEnumerable<ClubAdministrationDto>> GetPreviousAdministrationsOfUserAsync(string userId);
        Task<IEnumerable<ClubAdministrationStatusDto>> GetAdministrationStatuses(string userId);

        /// <summary>
        /// Get all members by specific Club
        /// </summary>
        /// <param name="clubId">The id of the Club</param>
        /// <returns>All members of a specific Club</returns>
        Task<IEnumerable<ClubMembersDto>> GetMembersByClubIdAsync(int clubId);

        /// <summary>
        /// Add follower to a specific Club
        /// </summary>
        /// <param name="clubId">The id of the Club</param>
        /// <param name="userId">The id of the user</param>
        /// <returns>An information about a new follower</returns>
        /// See <see cref="IClubParticipantsService.AddFollowerAsync(int, ClaimsPrincipal)"/> to add current user
        Task<ClubMembersDto> AddFollowerAsync(int clubId, string userId);

        /// <summary>
        /// Add follower to a specific Club
        /// </summary>
        /// <param name="clubId">The id of the Club</param>
        /// <param name="user">Current user</param>
        /// <returns>An information about a new follower</returns>
        /// See <see cref="IClubParticipantsService.AddFollowerAsync(int, string)"/> to add user by id
        Task<ClubMembersDto> AddFollowerAsync(int clubId, User user);

        /// <summary>
        /// Toggle approve status of a specific member
        /// </summary>
        /// <param name="memberId">The id of the member</param>
        /// <returns>An information about a specific member</returns>
        Task<ClubMembersDto> ToggleApproveStatusAsync(int memberId);

        /// <summary>
        /// Returns either given user is approved or not
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <returns>True if given user is approved, otherwise false</returns>
        /// See<see cref="ICityMembersService.CheckIsUserApproved(int)"/> to check if user is approved
        Task<bool?> CheckIsUserApproved(int userId);

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
        /// Remove a specific member from the Club
        /// </summary>
        /// <param name="userId">The Id of the member</param>
        Task RemoveMemberAsync(string userId);

        /// <summary>
        /// Add  follower from the Club to history table
        /// </summary>
        Task AddFollowerInHistoryAsync(int clubId, string userId);

        /// <summary>
        /// Add  follower from the Club to history table
        /// </summary>
        Task AddMemberInHistoryAsync(int clubId, string userId);

        /// <summary>
        /// update user in historical table insert property 
        /// </summary>
        Task UpdateStatusFollowerInHistoryAsync(string userId, bool isFollower, bool isDeleted);
    }
}