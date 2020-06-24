using EPlast.Bussiness.DTO.City;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.Bussiness.Interfaces.City
{
    public interface ICItyAdministrationService
    {
        Task<IEnumerable<CityAdministrationDTO>> GetByCityIdAsync(int cityId);
    }
}