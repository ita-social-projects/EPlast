using EPlast.BussinessLayer.DTO.City;
using System.Collections.Generic;

namespace EPlast.BussinessLayer.Interfaces.City
{
    public interface ICItyAdministrationService
    {
        IEnumerable<CityAdministrationDTO> GetByCityId(int cityAdministrationId);
    }
}
