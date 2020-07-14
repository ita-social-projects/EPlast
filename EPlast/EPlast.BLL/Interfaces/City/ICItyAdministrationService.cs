using EPlast.BLL.DTO.City;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.City
{
    public interface ICityAdministrationService
    {
        Task<IEnumerable<CityAdministrationDTO>> GetByCityIdAsync(int cityId);
        Task<CityAdministrationDTO> AddAdministratorAsync(CityAdministrationDTO adminDTO);
        Task<CityAdministrationDTO> EditAdministratorAsync(CityAdministrationDTO adminDTO);
        Task RemoveAdministratorAsync(int adminId);
    }
}