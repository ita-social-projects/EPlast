using EPlast.BLL.DTO.Club;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.Club
{
    /// <summary>
    ///  Implement  operations for work with clubs
    /// </summary>
    public interface IClubService
    {
        /// <summary>
        /// Get all clubs async
        /// </summary>
        /// <returns>Returns the IEnumerable of the club ClubDTO</returns>
        Task<IEnumerable<ClubDTO>> GetAllClubsAsync();

        /// <summary>
        /// Gets a specific number of clubs.
        /// </summary>
        /// <param name="pageNumber">A number of the page.</param>
        /// <param name="pageSize">A count of clubs to display.</param>
        /// <returns>Returns the IEnumerable of the club ClubDTO.</returns>
        Task<IEnumerable<ClubDTO>> GetPartOfClubsAsync(int pageNumber, int pageSize);

        /// <summary>
        /// Gets count of clubs.
        /// </summary>
        /// <returns>Returns count of clubs.</returns>
        Task<int> GetClubsCountAsync();

        /// <summary>
        /// Get club profile by club id
        /// </summary>
        /// <param name="clubId">Id of club</param>
        /// <returns>Returns the club profile (clubAdministration, members, followers) by ClubProfileDTO</returns>
        Task<ClubProfileDTO> GetClubProfileAsync(int clubId);

        /// <summary>
        /// Get club info by club id
        /// </summary>
        /// <param name="id">Id of club</param>
        /// <returns>Returns the ClubDTO with information about club</returns>
        Task<ClubDTO> GetClubInfoByIdAsync(int id);

        /// <summary>
        /// Get club members or followers by club id
        /// </summary>
        /// <param name="clubId">Id of club</param>
        /// <param name="isApproved">Search for members or followers </param>
        /// <returns>Returns the club profile with members or followers only</returns>
        Task<ClubProfileDTO> GetClubMembersOrFollowersAsync(int clubId, bool isApproved);

        /// <summary>
        /// Update club with new data
        /// </summary>
        /// <param name="club">Old club</param>
        /// <returns>New ClubDTO with updated data/returns>
        Task<ClubDTO> UpdateAsync(ClubDTO club);

        /// <summary>
        /// Create new club
        /// </summary>
        /// <param name="club">ClubDTO with club data</param>
        /// <returns>New ClubDTO with club id</returns>
        Task<ClubDTO> CreateAsync(ClubDTO club);

        /// <summary>
        /// Get image from blob storage
        /// </summary>
        /// <param name="imageName">Image name in blob storage</param>
        /// <returns>File as base64</returns>
        Task<string> GetImageBase64Async(string imageName);

        /// <summary>
        /// Validates a new club name.
        /// </summary>
        /// <param name="club">ClubDTO with club data.</param>
        /// <returns>Returns true if the club name is unique, otherwise - false.</returns>
        Task<bool> ValidateAsync(ClubDTO club);

        /// <summary>
        /// Verifies whether the club's name has not been changed compared to its value in the database.
        /// </summary>
        /// <param name="club">ClubDTO with club data.</param>
        /// <returns>Returns true if the club's name is not changed, otherwise - false.</returns>
        Task<bool> VerifyClubNameIsNotChangedAsync(ClubDTO club);

        /// <summary>
        /// Verifies whether the a user is able to join a club.
        /// </summary>
        /// <param name="clubId">Id of a club</param>
        /// <param name="userId">Id of a user</param>
        /// <returns>Returns true if the user is able to join, otherwise - false.</returns>
        Task<bool> VerifyUserCanJoinToClubAsync(int clubId, string userId);
    }
}