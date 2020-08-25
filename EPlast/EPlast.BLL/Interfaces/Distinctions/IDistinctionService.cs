using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EPlast.BLL
{
    public interface IDistinctionService
    {
        Task<IEnumerable<DistinctionDTO>> GetAllDistinctionAsync();
        Task<DistinctionDTO> GetDistinctionAsync(int id);
        Task AddDistinctionAsync(DistinctionDTO distinctionDTO, ClaimsPrincipal user);
        Task ChangeDistinctionAsync(DistinctionDTO distinctionDTO, ClaimsPrincipal user);
        Task DeleteDistinctionAsync(int id, ClaimsPrincipal user);
    }
}
