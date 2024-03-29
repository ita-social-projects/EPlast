using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using EPlast.BLL.DTO.Club;
using Microsoft.AspNetCore.Http;
using DataAccessClub = EPlast.DataAccess.Entities;

namespace EPlast.BLL.Interfaces.Club
{
    /// <summary>
    ///  Implement  operations for work with clubs
    /// </summary>
    public interface IClubService
    {

        /// <summary>
        /// Gets an information about a specific Club with 6 members per section
        /// </summary>
        /// <param name="clubId">The id of the Club</param>
        /// <returns>An information about a specific Club</returns>
        /// See <see cref="IClubService.GetClubProfileAsync(int, ClaimsPrincipal)"/> to get information about a specific Club including user roles
        Task<ClubProfileDto> GetClubProfileAsync(int clubId);

        /// <summary>
        /// Gets an information about a specific Club with 6 members per section
        /// </summary>
        /// <param name="clubId">The id of the Club</param>
        /// <param name="user">Current user</param>
        /// See <see cref="IClubService.GetClubProfileAsync(int)"/> to get information about a specific Club
        Task<ClubProfileDto> GetClubProfileAsync(int clubId, DataAccessClub.User user);

        /// <summary>
        /// Gets an information about members of the specific Club
        /// </summary>
        /// <param name="clubId">The id of the Club</param>
        /// <returns>An information about members of the specific Club</returns>
        Task<ClubProfileDto> GetClubMembersInfoAsync(int clubId);

        /// <summary>
        /// Gets a list of members of a specific Club
        /// </summary>
        /// <param name="clubId">The id of the Club</param>
        /// <returns>A list of members of a specific Club</returns>
        Task<ClubProfileDto> GetClubMembersAsync(int clubId);

        /// <summary>
        /// Gets a list of followers of a specific Club
        /// </summary>
        /// <param name="clubId">The id of the Club</param>
        /// <returns>A list of followers of a specific Club including user roles</returns>
        Task<ClubProfileDto> GetClubFollowersAsync(int clubId);

        /// <summary>
        /// Gets a list of administrators of a specific Club
        /// </summary>
        /// <param name="clubId">The id of the Club</param>
        /// <returns>A list of followers of a specific Club</returns>
        Task<ClubProfileDto> GetClubAdminsAsync(int clubId);

        /// <summary>
        /// Gets a list of documents of a specific Club
        /// </summary>
        /// <param name="clubId">The id of the Club</param>
        /// <returns>A list of documents of a specific Club</returns>
        Task<ClubProfileDto> GetClubDocumentsAsync(int clubId);

        /// <summary>
        /// Edits a specific Club
        /// </summary>
        /// <param name="clubId">The id of the Club</param>
        /// <returns>An information about an edited Club</returns>
        Task<ClubProfileDto> EditAsync(int clubId);

        /// <summary>
        /// Edits a specific Club
        /// </summary>
        /// <param name="model">An information about an edited Club</param>
        /// <param name="file">A new Club image</param>
        /// <returns>An information about an edited Club</returns>
        Task EditAsync(ClubProfileDto model, IFormFile file);

        /// <summary>
        /// Edits a specific Club
        /// </summary>
        /// <param name="model">An information about an edited Club</param>
        /// <returns>An information about an edited Club</returns>
        Task EditAsync(ClubDto model);

        /// <summary>
        /// Creates a new Club
        /// </summary>
        /// <param name="model">An information about a new Club</param>
        /// <param name="file">A new Club image</param>
        /// <returns>The id of a new Club</returns>
        Task<int> CreateAsync(ClubProfileDto model, IFormFile file);

        /// <summary>
        /// Creates a new Club
        /// </summary>
        /// <param name="model">An information about a new Club</param>
        /// <returns>The id of a new Club</returns>
        Task<int> CreateAsync(ClubDto model);

        /// <summary>
        /// Removes a specific Club
        /// </summary>
        /// <param name="clubId">The id of the Club</param>
        Task RemoveAsync(int clubId);

        /// <summary>
        /// Gets a photo in base64 format
        /// </summary>
        /// <param name="logoName">The name of a Club logo</param>
        /// <returns>A base64 string of the Club logo</returns>
        Task<string> GetLogoBase64(string logoName);


        /// <summary>
        /// Gets all clubs
        /// </summary>
        /// <returns>All clubs</returns>
        Task<IEnumerable<ClubForAdministrationDto>> GetClubs();

        /// <summary>
        /// Get all users of a specific club
        /// /// </summary>
        /// <param name="clubId">The id of the club</param>
        Task<IEnumerable<ClubUserDto>> GetClubUsersAsync(int clubId);

        Task DeleteClubMemberHistory(int id);
    }
}