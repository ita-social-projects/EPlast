using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.BLL.DTO.City;
using EPlast.DataAccess.Entities;

namespace EPlast.BLL.Interfaces.City
{
    public interface ICityAccessService
    {
        Task<IEnumerable<CityDto>> GetCitiesAsync(User user);
        Task<IEnumerable<CityForAdministrationDto>> GetAllCitiesIdAndName(User user);
        Task<bool> HasAccessAsync(User user, int cityId);
        Task<bool> HasAccessAsync(User user);
    }
}
