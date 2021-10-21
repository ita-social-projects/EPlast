using EPlast.BLL.DTO.City;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using DataAccessCity = EPlast.DataAccess.Entities;

namespace EPlast.BLL.Interfaces.City
{
    public interface ICityService
    {

        /// <summary>
        /// Archives a specific city
        /// </summary>
        /// <param name="cityId">The id of the city</param>
        Task ArchiveAsync(int cityId);

        /// <summary>
        /// Gets all cities
        /// </summary>
        /// <param name="cityName">Optional param to find cities by name</param>
        /// <returns>All cities of type City</returns>
        Task<IEnumerable<DataAccessCity.City>> GetAllAsync(string cityName = null);

        /// <summary>
        /// Gets all active cities
        /// </summary>
        /// <param name="cityName">Optional param to find cities by name</param>
        /// <returns>All active cities of type City</returns>
        Task<IEnumerable<DataAccessCity.City>> GetAllActiveAsync(string cityName = null);

        /// <summary>
        /// Gets all not active cities
        /// </summary>
        /// <param name="cityName">Optional param to find cities by name</param>
        /// <returns>All not active cities of type City</returns>
        Task<IEnumerable<DataAccessCity.City>> GetAllNotActiveAsync(string cityName = null);

        /// <summary>
        /// Gets all cities
        /// </summary>
        /// <param name="cityName">Optional param to find cities by name</param>
        /// <returns>All cities of type CityDTO</returns>
        Task<IEnumerable<CityDTO>> GetAllCitiesAsync(string cityName = null);

        /// <summary>
        /// Gets all active cities
        /// </summary>
        /// <param name="cityName">Optional param to find cities by name</param>
        /// <returns>All active cities of type CityDTO</returns>
        Task<IEnumerable<CityDTO>> GetAllActiveCitiesAsync(string cityName = null);

        /// <summary>
        /// Gets all not active cities
        /// </summary>
        /// <param name="cityName">Optional param to find cities by name</param>
        /// <returns>All not active cities of type CityDTO</returns>
        Task<IEnumerable<CityDTO>> GetAllNotActiveCitiesAsync(string cityName = null);

        /// <summary>
        /// Gets a list of cities by region
        /// </summary>
        /// <param name="regionId">The id of the region</param>
        /// <returns>List of cities by region</returns>
        Task<IEnumerable<CityDTO>> GetCitiesByRegionAsync(int regionId);

        /// <summary>
        /// Gets a specific city
        /// </summary>
        /// <param name="cityId">The id of the city</param>
        /// <returns></returns>
        Task<CityDTO> GetByIdAsync(int cityId);

        /// <summary>
        /// Gets an information about a specific city with 6 members per section
        /// </summary>
        /// <param name="cityId">The id of the city</param>
        /// <returns>An information about a specific city</returns>
        /// See <see cref="ICityService.GetCityProfileAsync(int, ClaimsPrincipal)"/> to get information about a specific city including user roles
        Task<CityProfileDTO> GetCityProfileAsync(int cityId);

        /// <summary>
        /// Gets an information about a specific city with 6 members per section
        /// </summary>
        /// <param name="cityId">The id of the city</param>
        /// <param name="user">Current user</param>
        /// See <see cref="ICityService.GetCityProfileAsync(int)"/> to get information about a specific city
        Task<CityProfileDTO> GetCityProfileAsync(int cityId, DataAccessCity.User user);

        /// <summary>
        /// Gets a list of members of a specific city
        /// </summary>
        /// <param name="cityId">The id of the city</param>
        /// <returns>A list of members of a specific city</returns>
        Task<CityProfileDTO> GetCityMembersAsync(int cityId);

        /// <summary>
        /// Gets a list of followers of a specific city
        /// </summary>
        /// <param name="cityId">The id of the city</param>
        /// <returns>A list of followers of a specific city including user roles</returns>
        Task<CityProfileDTO> GetCityFollowersAsync(int cityId);

        /// <summary>
        /// Gets a list of administrators of a specific city
        /// </summary>
        /// <param name="cityId">The id of the city</param>
        /// <returns>A list of followers of a specific city</returns>
        Task<CityProfileDTO> GetCityAdminsAsync(int cityId);

        /// <summary>
        /// Gets a list of documents of a specific city
        /// </summary>
        /// <param name="cityId">The id of the city</param>
        /// <returns>A list of documents of a specific city</returns>
        Task<CityProfileDTO> GetCityDocumentsAsync(int cityId);

        /// <summary>
        /// Edits a specific city
        /// </summary>
        /// <param name="cityId">The id of the city</param>
        /// <returns>An information about an edited city</returns>
        Task<CityProfileDTO> EditAsync(int cityId);

        /// <summary>
        /// Edits a specific city
        /// </summary>
        /// <param name="model">An information about an edited city</param>
        /// <param name="file">A new city image</param>
        /// <returns>An information about an edited city</returns>
        Task EditAsync(CityProfileDTO model, IFormFile file);

        /// <summary>
        /// Edits a specific city
        /// </summary>
        /// <param name="model">An information about an edited city</param>
        /// <returns>An information about an edited city</returns>
        Task EditAsync(CityDTO model);

        /// <summary>
        /// Creates a new city
        /// </summary>
        /// <param name="model">An information about a new city</param>
        /// <param name="file">A new city image</param>
        /// <returns>The id of a new city</returns>
        Task<int> CreateAsync(CityProfileDTO model, IFormFile file);

        /// <summary>
        /// Creates a new city
        /// </summary>
        /// <param name="model">An information about a new city</param>
        /// <returns>The id of a new city</returns>
        Task<int> CreateAsync(CityDTO model);

        /// <summary>
        /// Removes a specific city
        /// </summary>
        /// <param name="cityId">The id of the city</param>
        Task RemoveAsync(int cityId);

        /// <summary>
        /// Gets a photo in base64 format
        /// </summary>
        /// <param name="logoName">The name of a city logo</param>
        /// <returns>A base64 string of the city logo</returns>
        Task<string> GetLogoBase64(string logoName);

        /// <summary>
        /// Gets all cities
        /// </summary>
        /// <returns>All cities</returns>
        Task<IEnumerable<CityForAdministrationDTO>> GetCities();

        /// <summary>
        /// Unarchives a specific city
        /// </summary>
        /// <param name="cityId">The id of the city</param>
        Task UnArchiveAsync(int cityId);

        /// <summary>
        /// Get all users of a specific city
        /// </summary>
        /// <param name="cityId">The id of the city</param>
        Task<IEnumerable<CityUserDTO>> GetCityUsersAsync(int cityId);
    }
}