using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL
{
    interface IUserDistinctionService
    {
        Task<IEnumerable<UserDistinctionDTO>> GetAllUsersDistinctionAsync();
        Task<UserDistinctionDTO> GetUserDistinction(int id);
        UserDistinctionDTO AddUserDistinction();
        Task<bool> ChangeUserDistinction(UserDistinctionDTO userDistinctionDTO);
        Task<bool> DeleteUserDistinction(int id);        
    }
}
