using EPlast.BussinessLayer.DTO;
using EPlast.DataAccess.Entities;
using System.Collections.Generic;

namespace EPlast.BussinessLayer.Services.Interfaces
{
    public interface ICityService
    {
        IEnumerable<City> GetAll();
        IEnumerable<CityDTO> GetAllDTO();
    }
}
