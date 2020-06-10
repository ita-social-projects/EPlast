using EPlast.BussinessLayer.DTO.City;
using System.Collections.Generic;
Updateusing System.Threading.Tasks;

namespace EPlast.BussinessLayer.Interfaces.City
{
    public interface ICItyAdministrationService
    {
        Task<IEnumerable<CityAdministrationDTO>> GetByCityIdAsync(int cityAdministrationId);
    }
}
