using EPlast.BussinessLayer.DTO.City;
using System.Collections.Generic;

namespace EPlast.BussinessLayer.Services.Interfaces
{
    public interface ICityMembersService
    {
        IEnumerable<CityMembersDTO> GetCurrentByCityId(int cityId);
    }
}