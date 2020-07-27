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
        Task<IEnumerable<DataAccessCity.City>> GetAllAsync();
        Task<IEnumerable<CityDTO>> GetAllDTOAsync();
        Task<IEnumerable<CityDTO>> GetCitiesByRegionAsync(int regionId);
        Task<CityDTO> GetByIdAsync(int cityId);
        Task<CityProfileDTO> GetCityProfileAsync(int cityId);
        Task<CityProfileDTO> GetCityProfileAsync(int cityId, ClaimsPrincipal user);
        Task<CityProfileDTO> GetCityMembersAsync(int cityId);
        Task<CityProfileDTO> GetCityMembersAsync(int cityId, ClaimsPrincipal user);
        Task<CityProfileDTO> GetCityFollowersAsync(int cityId);
        Task<CityProfileDTO> GetCityFollowersAsync(int cityId, ClaimsPrincipal user);
        Task<CityProfileDTO> GetCityAdminsAsync(int cityId);
        Task<CityProfileDTO> GetCityAdminsAsync(int cityId, ClaimsPrincipal user);
        Task<CityProfileDTO> GetCityDocumentsAsync(int cityId);
        Task<CityProfileDTO> EditAsync(int cityId);
        Task EditAsync(CityProfileDTO model, IFormFile file);
        Task EditAsync(CityDTO model);
        Task<int> CreateAsync(CityProfileDTO model, IFormFile file);
        Task<int> CreateAsync(CityDTO model);
        Task<string> GetLogoBase64(string logoName);
    }
}