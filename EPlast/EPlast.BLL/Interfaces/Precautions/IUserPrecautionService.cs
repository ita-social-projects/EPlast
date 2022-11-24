using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.BLL.DTO.PrecautionsDTO;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.UserEntities;

namespace EPlast.BLL
{
    public interface IUserPrecautionService
    {
        Task<UserPrecautionsTableInfo> GetUserPrecautionsForTableAsync(PrecautionTableSettings tableSettings);
        Task<IEnumerable<UserPrecautionDto>> GetAllUsersPrecautionAsync();
        Task<UserPrecautionDto> GetUserPrecautionAsync(int id);
        Task<bool> AddUserPrecautionAsync(UserPrecautionDto userPrecautionDTO, User user);
        Task<bool> ChangeUserPrecautionAsync(UserPrecautionDto userPrecautionDTO, User user);
        Task<bool> DeleteUserPrecautionAsync(int id, User user);
        Task<IEnumerable<UserPrecautionDto>> GetUserPrecautionsOfUserAsync(string UserId);
        Task<UserPrecautionDto> GetUserActivePrecaution(string userId, string type);
        Task<bool> DoesNumberExistAsync(int number);
        Task<bool> DoesPrecautionExistAsync(int id);
        Task<IEnumerable<ShortUserInformationDto>> UsersTableWithoutPrecautionAsync();
        Task<bool> CheckUserPrecautionsType(string userId, string type);
        Task<IEnumerable<SuggestedUserDto>> GetUsersForPrecautionAsync(User currentUser);
    }
}
