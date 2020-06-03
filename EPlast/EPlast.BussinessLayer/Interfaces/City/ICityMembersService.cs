using EPlast.BussinessLayer.DTO.City;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BussinessLayer.Services.Interfaces
{
    public interface ICityMembersService
    {
        Task<IEnumerable<CityMembersDTO>> GetCurrentByCityIdAsync(int cityId);
    }
}