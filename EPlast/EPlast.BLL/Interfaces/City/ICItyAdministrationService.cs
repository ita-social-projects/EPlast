using EPlast.BLL.DTO.City;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.City
{
    public interface ICityAdministrationService
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
        Task CheckPreviousAdministratorsToDelete();

        /// <summary>
        ///returns administrations of given user
        /// </summary>
        Task<IEnumerable<CityAdministrationDTO>> GetAdministrationsOfUser(string UserId);
    }
}