using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EPlast.BLL
{
    public interface IUserDistinctionService
    {
        Task<IEnumerable<UserDistinctionDTO>> GetAllUsersDistinctionAsync();
        Task<UserDistinctionDTO> GetUserDistinction(int id);
        Task AddUserDistinction(UserDistinctionDTO userDistinctionDTO, ClaimsPrincipal user);
        Task ChangeUserDistinction(UserDistinctionDTO userDistinctionDTO, ClaimsPrincipal user);
        Task DeleteUserDistinction(int id, ClaimsPrincipal user);
        Task<IEnumerable<UserDistinctionDTO>> GetUserDistinctionsOfGivenUser(string UserId);
    }
}
