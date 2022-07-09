using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using EPlast.DataAccess.Entities;

namespace EPlast.BLL
{
    public interface IUserDistinctionService
    {
        Task<IEnumerable<UserDistinctionDto>> GetAllUsersDistinctionAsync();
        Task<UserDistinctionDto> GetUserDistinctionAsync(int id);
        Task AddUserDistinctionAsync(UserDistinctionDto userDistinctionDTO, User user);
        Task ChangeUserDistinctionAsync(UserDistinctionDto userDistinctionDTO, User user);
        Task DeleteUserDistinctionAsync(int id, User user);
        Task<IEnumerable<UserDistinctionDto>> GetUserDistinctionsOfUserAsync(string UserId);
        Task<bool> IsNumberExistAsync(int number);
    }
}
