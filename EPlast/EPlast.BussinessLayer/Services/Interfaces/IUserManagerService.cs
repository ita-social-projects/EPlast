using EPlast.BussinessLayer.DTO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EPlast.BussinessLayer.Services.Interfaces
{
    public interface IUserManagerService
    {
        Task<bool> IsInRole(UserDTO user, params string[] roles);
        Task<bool> IsInRole(ClaimsPrincipal user, params string[] roles);
        string GetUserId(ClaimsPrincipal user);
    }
}
