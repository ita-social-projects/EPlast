using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EPlast.BLL
{
    public interface IUserDistinctionService
    {
        Task<IEnumerable<UserDistinctionDTO>> GetAllUsersDistinctionAsync();
        Task<UserDistinctionDTO> GetUserDistinctionAsync(int id);
        Task AddUserDistinctionAsync(UserDistinctionDTO userDistinctionDTO, ClaimsPrincipal user);
        Task ChangeUserDistinctionAsync(UserDistinctionDTO userDistinctionDTO, ClaimsPrincipal user);
        Task DeleteUserDistinctionAsync(int id, ClaimsPrincipal user);
        Task<IEnumerable<UserDistinctionDTO>> GetUserDistinctionsOfUserAsync(string UserId);
        Task<bool> IsNumberExistAsync(int number);
    }
}
