using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using EPlast.DataAccess.Entities;

namespace EPlast.BLL
{
    public interface IDistinctionService
    {
        Task<IEnumerable<DistinctionDTO>> GetAllDistinctionAsync();
        Task<DistinctionDTO> GetDistinctionAsync(int id);
        Task AddDistinctionAsync(DistinctionDTO distinctionDTO, User user);
        Task ChangeDistinctionAsync(DistinctionDTO distinctionDTO, User user);
        Task DeleteDistinctionAsync(int id, User user);
    }
}
