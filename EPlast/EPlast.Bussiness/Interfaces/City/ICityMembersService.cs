using EPlast.Bussiness.DTO.City;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.Bussiness.Services.Interfaces
{
    public interface ICityMembersService
    {
        Task<IEnumerable<CityMembersDTO>> GetCurrentByCityIdAsync(int cityId);
    }
}