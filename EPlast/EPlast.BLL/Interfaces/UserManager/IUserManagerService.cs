using EPlast.BLL.DTO.UserProfiles;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.Interfaces
{
    public interface IUserManagerService
    {
        Task<bool> IsInRoleAsync(UserDTO user, params string[] roles);
        //Task<bool> IsInRoleAsync(ClaimsPrincipal user, params string[] roles);
        //Task<string> GetUserIdAsync(ClaimsPrincipal user);
        Task<UserDTO> FindByIdAsync(string userId);
        Task<IEnumerable<string>> GetRolesAsync(UserDTO user);
    }
}
