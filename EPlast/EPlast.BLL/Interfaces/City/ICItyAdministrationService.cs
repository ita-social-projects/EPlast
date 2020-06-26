using EPlast.BLL.DTO.City;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.City
{
    public interface ICItyAdministrationService
    {
        Task<IEnumerable<CityAdministrationDTO>> GetByCityIdAsync(int cityId);
    }
}