using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.DataAccess.Entities;
using EPlast.BLL.DTO;

namespace EPlast.BLL
{
    public interface IUserPrecautionService
    {
        Task<IEnumerable<UserPrecautionDTO>> GetAllUsersPrecautionAsync();
        Task<UserPrecautionDTO> GetUserPrecautionAsync(int id);
        Task AddUserPrecautionAsync(UserPrecautionDTO userPrecautionDTO, User user);
        Task ChangeUserPrecautionAsync(UserPrecautionDTO userPrecautionDTO, User user);
        Task DeleteUserPrecautionAsync(int id, User user);
        Task<IEnumerable<UserPrecautionDTO>> GetUserPrecautionsOfUserAsync(string UserId);
        Task<bool> IsNumberExistAsync(int number);
        Task<IEnumerable<UserTableDTO>> UsersTableWithoutPrecautionAsync();
    }
}
