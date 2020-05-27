using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BussinessLayer.Services.Interfaces
{
    public interface ICityAccessService
    {
        Task<IEnumerable<DatabaseEntities.City>> GetCities(ClaimsPrincipal claimsPrincipal);
        Task<bool> HasAccess(ClaimsPrincipal claimsPrincipal, int cityId);
    }
}