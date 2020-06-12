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
        CityDTO GetById(int cityId);
        CityProfileDTO CityProfile(int cityId);
        CityProfileDTO CityMembers(int cityId);
        CityProfileDTO CityFollowers(int cityId);
        CityProfileDTO CityAdmins(int cityId);
        CityProfileDTO CityDocuments(int cityId);
        CityProfileDTO Edit(int cityId);
        void Edit(CityProfileDTO model, IFormFile file);
        int Create(CityProfileDTO model, IFormFile file);

    }
}
