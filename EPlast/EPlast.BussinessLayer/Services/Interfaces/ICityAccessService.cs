using EPlast.BussinessLayer.DTO.City;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EPlast.BussinessLayer.Services.Interfaces
{
    public interface ICityAccessService
    {
        Task<IEnumerable<CityDTO>> GetCities(ClaimsPrincipal claimsPrincipal);
        Task<bool> HasAccess(ClaimsPrincipal claimsPrincipal, int cityId);
    }
}