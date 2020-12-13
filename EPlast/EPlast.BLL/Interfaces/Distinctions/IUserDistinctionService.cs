using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using EPlast.DataAccess.Entities;

namespace EPlast.BLL
{
    public interface IUserDistinctionService
    {
        Task<IEnumerable<UserDistinctionDTO>> GetAllUsersDistinctionAsync();
        Task<UserDistinctionDTO> GetUserDistinctionAsync(int id);
        Task AddUserDistinctionAsync(UserDistinctionDTO userDistinctionDTO, User user);
        Task ChangeUserDistinctionAsync(UserDistinctionDTO userDistinctionDTO, User user);
        Task DeleteUserDistinctionAsync(int id, User user);
        Task<IEnumerable<UserDistinctionDTO>> GetUserDistinctionsOfUserAsync(string UserId);
        Task<bool> IsNumberExistAsync(int number);
    }
}
