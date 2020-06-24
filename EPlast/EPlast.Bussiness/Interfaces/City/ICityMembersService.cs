using EPlast.BusinessLogicLayer.DTO.City;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BusinessLogicLayer.Services.Interfaces
{
    public interface ICityMembersService
    {
        Task<IEnumerable<CityMembersDTO>> GetCurrentByCityIdAsync(int cityId);
    }
}