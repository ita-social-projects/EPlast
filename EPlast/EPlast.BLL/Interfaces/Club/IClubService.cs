using EPlast.BLL.DTO.Club;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using DataAccessClub = EPlast.DataAccess.Entities;

namespace EPlast.BLL.Interfaces.Club
{
    /// <summary>
    ///  Implement  operations for work with clubs
    /// </summary>
    public interface IClubService
    {
        /// <summary>
        /// Get all cities
        /// </summary>
        /// <param name="ClubName">Optional param to find cities by name</param>
        /// <returns>All cities of type Club</returns>
        Task<IEnumerable<DataAccessClub.Club>> GetAllAsync(string ClubName = null);

        /// <summary>
        /// Get all cities
        /// </summary>
        /// <param name="ClubName">Optional param to find cities by name</param>
        /// <returns>All cities of type ClubDTO</returns>
        Task<IEnumerable<ClubDTO>> GetAllDTOAsync(string ClubName = null);

        /// <summary>
        /// Get a list of cities by region
        /// </summary>
        /// <param name="regionId">The id of the region</param>
        /// <returns>List of cities by region</returns>
        //Task<IEnumerable<ClubDTO>> GetCitiesByRegionAsync(int regionId);

        /// <summary>
        /// Get a specific Club
        /// </summary>
        /// <param name="ClubId">The id of the Club</param>
        /// <returns></returns>
        Task<ClubDTO> GetByIdAsync(int ClubId);

        /// <summary>
        /// Get an information about a specific Club with 6 members per section
        /// </summary>
        /// <param name="ClubId">The id of the Club</param>
        /// <returns>An information about a specific Club</returns>
        /// See <see cref="IClubService.GetClubProfileAsync(int, ClaimsPrincipal)"/> to get information about a specific Club including user roles
        Task<ClubProfileDTO> GetClubProfileAsync(int ClubId);

        /// <summary>
        /// Get an information about members of the specific Club
        /// </summary>
        /// <param name="ClubId">The id of the Club</param>
        /// <returns>An information about members of the specific Club</returns>
        Task<ClubProfileDTO> GetClubMembersInfoAsync(int ClubId);

        /// <summary>
        /// Get an information about a specific Club with 6 members per section
        /// </summary>
        /// <param name="ClubId">The id of the Club</param>
        /// <param name="user">Current user</param>
        /// See <see cref="IClubService.GetClubProfileAsync(int)"/> to get information about a specific Club
        Task<ClubProfileDTO> GetClubProfileAsync(int ClubId, DataAccessClub.User user);

        /// <summary>
        /// Get a list of members of a specific Club
        /// </summary>
        /// <param name="ClubId">The id of the Club</param>
        /// <returns>A list of members of a specific Club</returns>
        Task<ClubProfileDTO> GetClubMembersAsync(int ClubId);

        /// <summary>
        /// Get a list of followers of a specific Club
        /// </summary>
        /// <param name="ClubId">The id of the Club</param>
        /// <returns>A list of followers of a specific Club including user roles</returns>
        Task<ClubProfileDTO> GetClubFollowersAsync(int ClubId);

        /// <summary>
        /// Get a list of administrators of a specific Club
        /// </summary>
        /// <param name="ClubId">The id of the Club</param>
        /// <returns>A list of followers of a specific Club</returns>
        Task<ClubProfileDTO> GetClubAdminsAsync(int ClubId);

        /// <summary>
        /// Get a list of documents of a specific Club
        /// </summary>
        /// <param name="ClubId">The id of the Club</param>
        /// <returns>A list of documents of a specific Club</returns>
        Task<ClubProfileDTO> GetClubDocumentsAsync(int ClubId);

        /// <summary>
        /// Edit a specific Club
        /// </summary>
        /// <param name="ClubId">The id of the Club</param>
        /// <returns>An information about an edited Club</returns>
        Task<ClubProfileDTO> EditAsync(int ClubId);

        /// <summary>
        /// Edit a specific Club
        /// </summary>
        /// <param name="model">An information about an edited Club</param>
        /// <param name="file">A new Club image</param>
        /// <returns>An information about an edited Club</returns>
        Task EditAsync(ClubProfileDTO model, IFormFile file);

        /// <summary>
        /// Edit a specific Club
        /// </summary>
        /// <param name="model">An information about an edited Club</param>
        /// <returns>An information about an edited Club</returns>
        Task EditAsync(ClubDTO model);

        /// <summary>
        /// Create a new Club
        /// </summary>
        /// <param name="model">An information about a new Club</param>
        /// <param name="file">A new Club image</param>
        /// <returns>The id of a new Club</returns>
        Task<int> CreateAsync(ClubProfileDTO model, IFormFile file);

        /// <summary>
        /// Create a new Club
        /// </summary>
        /// <param name="model">An information about a new Club</param>
        /// <returns>The id of a new Club</returns>
        Task<int> CreateAsync(ClubDTO model);

        /// <summary>
        /// Remove a specific Club
        /// </summary>
        /// <param name="ClubId">The id of the Club</param>
        Task RemoveAsync(int ClubId);

        /// <summary>
        /// Get a photo in base64 format
        /// </summary>
        /// <param name="logoName">The name of a Club logo</param>
        /// <returns>A base64 string of the Club logo</returns>
        Task<string> GetLogoBase64(string logoName);


        /// <summary>
        /// Get all clubs
        /// </summary>
        /// <returns>All clubs</returns>
        Task<IEnumerable<ClubForAdministrationDTO>> GetClubs();
    }
}