using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.BLL.DTO.City;
using EPlast.DataAccess.Entities;

namespace EPlast.BLL.Interfaces.City
{
    public interface ICityParticipantsService
    {
        /// <summary>
        /// Get an information about a specific administrator
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns>An information about a specific administrator</returns>
        Task<IEnumerable<CityAdministrationDTO>> GetAdministrationByIdAsync(int cityId);

        /// <summary>
        /// Add a new administrator to the city
        /// </summary>
        /// <param name="adminDTO">An information about a new administrator</param>
        /// <returns>An information about a specific administrator</returns>
        Task<CityAdministrationDTO> AddAdministratorAsync(CityAdministrationDTO adminDTO);

        /// <summary>
        /// Edit an information about a specific admininstrator
        /// </summary>
        /// <param name="adminDTO">An information about an edited administrator</param>
        /// <returns>An information about a specific admininstrator</returns>
        Task<CityAdministrationDTO> EditAdministratorAsync(CityAdministrationDTO adminDTO);

        /// <summary>
        /// Remove a specific administrator from the city
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
        Task<IEnumerable<CityAdministrationDTO>> GetAdministrationsOfUserAsync(string UserId);

        Task<IEnumerable<CityAdministrationStatusDTO>> GetAdministrationStatuses(string UserId);

        /// <summary>
        ///returns previous administrations of given user
        /// </summary>
        Task<IEnumerable<CityAdministrationDTO>> GetPreviousAdministrationsOfUserAsync(string UserId);
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
        /// Returns either given user is approved or not
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <returns>True if given user is approved, otherwise false</returns>
        /// See<see cref="ICityMembersService.IsUserApproved(int)"/> to check if user is approved
        Task<bool> IsUserApproved(int userId);

        /// <summary>
        /// Toggle approve status of a specific member
        /// </summary>
        /// <param name="memberId">The id of the member</param>
        /// <returns>An information about a specific member</returns>
        Task<CityMembersDTO> ToggleApproveStatusAsync(int memberId);

        /// <summary>
        /// City name only for approved member
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns>city name string</returns>
        Task<string> CityOfApprovedMember(string memberId);

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
