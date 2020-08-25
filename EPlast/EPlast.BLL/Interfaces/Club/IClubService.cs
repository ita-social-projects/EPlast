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

        Task<bool> Validate(ClubDTO club);
        Task<bool> VerifyClubNameIsNotChanged(ClubDTO club);
    }
}