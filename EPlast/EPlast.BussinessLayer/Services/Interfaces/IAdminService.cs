using EPlast.BussinessLayer.DTO;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BussinessLayer.Services.Interfaces
{
    public interface IAdminService
    {
        IEnumerable<IdentityRole> GetRolesExceptAdmin();
        Task Edit(string userId, List<string> roles);
        Task DeleteUser(string userId);
        Task<IEnumerable<UserTableDTO>> UsersTable();
    }
}
