using EPlast.BussinessLayer.DTO.City;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EPlast.BussinessLayer.Services.Interfaces
{
    public interface ICityAccessService
    {
        Task<IEnumerable<CityDTO>> GetCitiesAsync(ClaimsPrincipal claimsPrincipal);
        Task<bool> HasAccessAsync(ClaimsPrincipal claimsPrincipal, int cityId);
    }
}