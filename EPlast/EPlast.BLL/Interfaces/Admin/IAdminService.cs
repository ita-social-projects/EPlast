using EPlast.BLL.DTO;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.Interfaces
{
    public interface IAdminService
    {
        Task<IEnumerable<IdentityRole>> GetRolesExceptAdminAsync();
        Task EditAsync(string userId, List<string> roles);
        Task DeleteUserAsync(string userId);
        Task<IEnumerable<UserTableDTO>> UsersTableAsync();
    }
}
