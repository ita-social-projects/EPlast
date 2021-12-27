using EPlast.BLL.DTO.Club;
using Microsoft.AspNetCore.Http;
using System;
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
        /// Archives a specific club
        /// </summary>
        /// <param name="clubId">The id of the club</param>
        Task ArchiveAsync(int clubId);

        /// <summary>
        /// Gets all cities
        /// </summary>
        /// <param name="clubName">Optional param to find club by name</param>
        /// <returns>All clubs of type Club</returns>
        Task<IEnumerable<DataAccessClub.Club>> GetAllAsync(string clubName = null);

        /// <summary>
        /// Gets all clubs
        /// </summary>
        /// <param name="clubName">Optional param to find club by name</param>
        /// <returns>All active clubs of type Club</returns>
        Task<IEnumerable<DataAccessClub.Club>> GetAllActiveAsync(string clubName = null);

        /// <summary>
        /// Gets all clubs
        /// </summary>
        /// <param name="clubName">Optional param to find club by name</param>
        /// <returns>All not active clubs of type Club</returns>
        Task<IEnumerable<DataAccessClub.Club>> GetAllNotActiveAsync(string clubName = null);

        /// <summary>
        /// Gets all clubs
        /// </summary>
        /// <param name="clubName">Optional param to find club by name</param>
        /// <returns>All cities of type ClubDTO</returns>
        Task<IEnumerable<ClubDTO>> GetAllClubsAsync(string clubName = null);

        /// <summary>
        /// Gets all active clubs
        /// </summary>
        /// <param name="clubName">Optional param to find active club by name</param>
        /// <returns>All active cities of type ClubDTO</returns>
        Task<IEnumerable<ClubDTO>> GetAllActiveClubsAsync(string clubName = null);

        /// <summary>
        /// Gets all not active clubs
        /// </summary>
        /// <param name="clubName">Optional param to find not active club by name</param>
        /// <returns>All not active cities of type ClubDTO</returns>
        Task<IEnumerable<ClubDTO>> GetAllNotActiveClubsAsync(string clubName = null);

        /// <summary>
        /// Gets all regions based on page and their archivation status
        /// </summary>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Size of the page</param>
        /// /// <param name="clubName">Search string to find club by name</param>
        /// /// <param name="isArchive">Archivation status of the club</param>
        /// <returns>All not active cities of type ClubDTO</returns>
        Task<Tuple<IEnumerable<ClubObjectDTO>, int>> GetAllClubsByPageAndIsArchiveAsync(int page, int pageSize, string clubName, bool isArchive);

        /// <summary>
        /// Gets a specific Club
        /// </summary>
        /// <param name="clubId">The id of the Club</param>
        /// <returns></returns>
        Task<ClubDTO> GetByIdAsync(int clubId);

        /// <summary>
        /// Gets an information about a specific Club with 6 members per section
        /// </summary>
        /// <param name="clubId">The id of the Club</param>
        /// <returns>An information about a specific Club</returns>
        /// See <see cref="IClubService.GetClubProfileAsync(int, ClaimsPrincipal)"/> to get information about a specific Club including user roles
        Task<ClubProfileDTO> GetClubProfileAsync(int clubId);

        /// <summary>
        /// Gets an information about members of the specific Club
        /// </summary>
        /// <param name="clubId">The id of the Club</param>
        /// <returns>An information about members of the specific Club</returns>
        Task<ClubProfileDTO> GetClubMembersInfoAsync(int clubId);

        /// <summary>
        /// Gets an information about a specific Club with 6 members per section
        /// </summary>
        /// <param name="clubId">The id of the Club</param>
        /// <param name="user">Current user</param>
        /// See <see cref="IClubService.GetClubProfileAsync(int)"/> to get information about a specific Club
        Task<ClubProfileDTO> GetClubProfileAsync(int clubId, DataAccessClub.User user);

        /// <summary>
        /// Gets a list of members of a specific Club
        /// </summary>
        /// <param name="clubId">The id of the Club</param>
        /// <returns>A list of members of a specific Club</returns>
        Task<ClubProfileDTO> GetClubMembersAsync(int clubId);

        /// <summary>
        /// Gets a list of followers of a specific Club
        /// </summary>
        /// <param name="clubId">The id of the Club</param>
        /// <returns>A list of followers of a specific Club including user roles</returns>
        Task<ClubProfileDTO> GetClubFollowersAsync(int clubId);

        /// <summary>
        /// Gets a list of administrators of a specific Club
        /// </summary>
        /// <param name="clubId">The id of the Club</param>
        /// <returns>A list of followers of a specific Club</returns>
        Task<ClubProfileDTO> GetClubAdminsAsync(int clubId);

        /// <summary>
        /// Gets a list of documents of a specific Club
        /// </summary>
        /// <param name="clubId">The id of the Club</param>
        /// <returns>A list of documents of a specific Club</returns>
        Task<ClubProfileDTO> GetClubDocumentsAsync(int clubId);

        /// <summary>
        /// Edits a specific Club
        /// </summary>
        /// <param name="clubId">The id of the Club</param>
        /// <returns>An information about an edited Club</returns>
        Task<ClubProfileDTO> EditAsync(int clubId);

        /// <summary>
        /// Edits a specific Club
        /// </summary>
        /// <param name="model">An information about an edited Club</param>
        /// <param name="file">A new Club image</param>
        /// <returns>An information about an edited Club</returns>
        Task EditAsync(ClubProfileDTO model, IFormFile file);

        /// <summary>
        /// Edits a specific Club
        /// </summary>
        /// <param name="model">An information about an edited Club</param>
        /// <returns>An information about an edited Club</returns>
        Task EditAsync(ClubDTO model);

        /// <summary>
        /// Creates a new Club
        /// </summary>
        /// <param name="model">An information about a new Club</param>
        /// <param name="file">A new Club image</param>
        /// <returns>The id of a new Club</returns>
        Task<int> CreateAsync(ClubProfileDTO model, IFormFile file);

        /// <summary>
        /// Creates a new Club
        /// </summary>
        /// <param name="model">An information about a new Club</param>
        /// <returns>The id of a new Club</returns>
        Task<int> CreateAsync(ClubDTO model);

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
        Task<IEnumerable<ClubForAdministrationDTO>> GetClubs();

        /// <summary>
        /// Gets all data for report clubs
        /// </summary>
        Task<ClubReportDataDTO> GetClubDataForReport(int clubId);

        /// <summary>
        /// Gets all club followers from history
        /// </summary>
        Task<IEnumerable<ClubMemberHistoryDTO>> GetClubHistoryFollowers(int clubId);

        /// <summary>
        ///  Gets all club members from history
        /// </summary>
        Task<IEnumerable<ClubMemberHistoryDTO>> GetClubHistoryMembers(int clubId);

        /// <summary>
        /// Gets all club administrations
        /// </summary>
        Task<IEnumerable<DataAccessClub.ClubAdministration>> GetClubAdministrations(int clubId);

        /// <summary>
        /// Gets count of users per year
        /// </summary>
        /// <returns>count of users</returns>
        Task<int> GetCountUsersPerYear(int clubId);

        /// <summary>
        /// Gets count of deleted users 
        /// </summary>
        /// <returns>count of deletet users</returns>
        Task<int> GetCountDeletedUsersPerYear(int clubId);

        /// <summary>
        /// Unarchives a specific club
        /// </summary>
        /// <param name="clubId">The id of the club</param>
        Task UnArchiveAsync(int clubId);

        /// <summary>
        /// Get all users of a specific club
        /// /// </summary>
        /// <param name="clubId">The id of the club</param>
        Task<IEnumerable<ClubUserDTO>> GetClubUsersAsync(int clubId);

        Task DeleteClubMemberHistory(int id);
    }
}