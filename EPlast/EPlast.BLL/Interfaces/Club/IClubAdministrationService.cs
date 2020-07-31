using EPlast.BLL.DTO.Club;
using System;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.Club
{
    public interface IClubAdministrationService
    {
        /// <summary>
        /// Get club administration
        /// </summary>
        /// <param name="clubId">Club id</param>
        /// <returns>ClubProfileDTO with administration</returns>
        Task<ClubProfileDTO> GetClubAdministrationByIdAsync(int clubId);

        /// <summary>
        /// Remove user from club administration
        /// </summary>
        /// <param name="id">User-club id</param>
        /// <returns>Bool value with status of operation</returns>
        Task<bool> DeleteClubAdminAsync(int id);

        /// <summary>
        /// Set admin qualification end date
        /// </summary>
        /// <param name="clubAdministrationId">User admin id</param>
        /// <param name="endDate">End date</param>
        /// <returns>Updated ClubAdministrationDTO</returns>
        Task<ClubAdministrationDTO> SetAdminEndDateAsync(int clubAdministrationId, DateTime endDate);

        /// <summary>
        /// Add new admin to club
        /// </summary>
        /// <param name="createdAdmin"></param>
        /// <returns>Updated ClubAdministrationDTO</returns>
        Task<ClubAdministrationDTO> AddClubAdminAsync(ClubAdministrationDTO createdAdmin);
    }
}