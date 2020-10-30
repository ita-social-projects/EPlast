using EPlast.BLL.DTO.Club;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.Club
{
    public interface IClubAdministrationService
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
        Task CheckPreviousAdministratorsToDelete();

        /// <summary>
        ///returns administrations of given user
        /// </summary>
        Task<IEnumerable<ClubAdministrationDTO>> GetAdministrationsOfUserAsync(string UserId);

        /// <summary>
        ///returns administrations of given user
        /// </summary>
        Task<IEnumerable<ClubAdministrationDTO>> GetPreviousAdministrationsOfUserAsync(string UserId);
        Task<IEnumerable<ClubAdministrationStatusDTO>> GetAdministrationStatuses(string UserId);

    }
}