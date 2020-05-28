using EPlast.BussinessLayer.DTO.City;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using DataAccessCity = EPlast.DataAccess.Entities;

namespace EPlast.BussinessLayer.Interfaces.City
{
    public interface ICityService
    {
        IEnumerable<DataAccessCity.City> GetAll();
        IEnumerable<CityDTO> GetAllDTO();
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
