using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.DataAccess.Entities;
using EPlast.BLL.DTO.PrecautionsDTO;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.DataAccess.Entities.UserEntities;

namespace EPlast.BLL
{
    public interface IUserPrecautionService
    {
        Task<UserPrecautionsTableInfo> GetUserPrecautionsForTableAsync(PrecautionTableSettings tableSettings);
        Task<IEnumerable<UserPrecautionDTO>> GetAllUsersPrecautionAsync();
        Task<UserPrecautionDTO> GetUserPrecautionAsync(int id);
        Task<bool> AddUserPrecautionAsync(UserPrecautionDTO userPrecautionDTO, User user);
        Task<bool> ChangeUserPrecautionAsync(UserPrecautionDTO userPrecautionDTO, User user);
        Task<bool> DeleteUserPrecautionAsync(int id, User user);
        Task<IEnumerable<UserPrecautionDTO>> GetUserPrecautionsOfUserAsync(string UserId);
        Task<UserPrecautionDTO> GetUserActivePrecaution(string userId, string type);
        Task<bool> IsNumberExistAsync(int number, int? id = null);
        Task<IEnumerable<ShortUserInformationDTO>> UsersTableWithoutPrecautionAsync();
        Task<bool> CheckUserPrecautionsType(string userId, string type);
        Task<IEnumerable<SuggestedUserDto>> GetUsersForPrecautionAsync(User user);
    }
}
