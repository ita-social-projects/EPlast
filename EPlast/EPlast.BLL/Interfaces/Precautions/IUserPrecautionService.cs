using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.DataAccess.Entities;
using EPlast.BLL.DTO;
using EPlast.BLL.DTO.UserProfiles;

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
        Task<UserPrecautionDTO> GetUserActivePrecaution(string userId, string type);
        Task<bool> IsNumberExistAsync(int number, int? id = null);
        Task<IEnumerable<ShortUserInformationDTO>> UsersTableWithoutPrecautionAsync();
        Task<bool> CheckUserPrecautionsType(string userId, string type);
    }
}
