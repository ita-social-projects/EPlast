using EPlast.BussinessLayer.DTO.City;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccessCity = EPlast.DataAccess.Entities;

namespace EPlast.BussinessLayer.Interfaces.City
{
    public interface ICityService
    {
        Task<IEnumerable<DataAccessCity.City>> GetAllAsync();
        Task<IEnumerable<CityDTO>> GetAllDTOAsync();
        Task<CityDTO> GetByIdAsync(int cityId);
        Task<CityProfileDTO> CityProfileAsync(int cityId);
        Task<CityProfileDTO> CityMembersAsync(int cityId);
        Task<CityProfileDTO> CityFollowersAsync(int cityId);
        Task<CityProfileDTO> CityAdminsAsync(int cityId);
        Task<CityProfileDTO> CityDocumentsAsync(int cityId);
        Task<CityProfileDTO> EditAsync(int cityId);
        Task EditAsync(CityProfileDTO model, IFormFile file);
        Task<int> CreateAsync(CityProfileDTO model, IFormFile file);

    }
}