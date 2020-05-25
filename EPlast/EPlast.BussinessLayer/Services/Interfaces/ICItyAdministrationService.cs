using EPlast.BussinessLayer.DTO;
using System.Collections.Generic;

namespace EPlast.BussinessLayer.Services.Interfaces
{
    public interface ICItyAdministrationService
    {
        IEnumerable<CityAdministrationDTO> GetByCityId(int cityAdministrationId);
    }
}
