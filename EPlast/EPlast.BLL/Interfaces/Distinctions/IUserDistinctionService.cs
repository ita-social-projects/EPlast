using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL
{
    public interface IUserDistinctionService
    {
        Task<IEnumerable<UserDistinctionDTO>> GetAllUsersDistinctionAsync();
        Task<UserDistinctionDTO> GetUserDistinction(int id);
        Task AddUserDistinction(UserDistinctionDTO userDistinctionDTO);
        Task ChangeUserDistinction(UserDistinctionDTO userDistinctionDTO);
        Task DeleteUserDistinction(int id);        
    }
}
