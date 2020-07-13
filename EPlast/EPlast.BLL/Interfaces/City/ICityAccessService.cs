using EPlast.BLL.DTO.City;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.City
{
    public interface ICityAccessService
    {
        Task<IEnumerable<CityDTO>> GetCitiesAsync(ClaimsPrincipal claimsPrincipal);
        Task<bool> HasAccessAsync(ClaimsPrincipal claimsPrincipal, int cityId);
    }
}