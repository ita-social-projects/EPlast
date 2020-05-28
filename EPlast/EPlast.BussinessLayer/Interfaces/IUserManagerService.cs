using EPlast.BussinessLayer.DTO.UserProfiles;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EPlast.BussinessLayer.Services.Interfaces
{
    public interface IUserManagerService
    {
        Task<bool> IsInRole(UserDTO user, params string[] roles);
        Task<bool> IsInRole(ClaimsPrincipal user, params string[] roles);
        string GetUserId(ClaimsPrincipal user);
        Task<UserDTO> FindById(string userId);
        Task<IEnumerable<string>> GetRoles(UserDTO user);

    }
}
