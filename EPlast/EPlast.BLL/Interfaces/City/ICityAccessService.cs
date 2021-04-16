using System;
using EPlast.BLL.DTO.City;
using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.DataAccess.Entities;

namespace EPlast.BLL.Interfaces.City
{
    public interface ICityAccessService
    {
        Task<IEnumerable<CityDTO>> GetCitiesAsync(User user);
        Task<IEnumerable<Tuple<int, string>>> GetAllCitiesIdAndName(User user);
        Task<bool> HasAccessAsync(User user, int cityId);
        Task<bool> HasAccessAsync(User user);
    }
}
