using EPlast.BLL.DTO.City;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.Interfaces
{
    public interface ICityMembersService
    {
        Task<IEnumerable<CityMembersDTO>> GetCurrentByCityIdAsync(int cityId);
    }
}