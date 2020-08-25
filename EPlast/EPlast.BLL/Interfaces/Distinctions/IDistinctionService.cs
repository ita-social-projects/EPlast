using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EPlast.BLL
{
    public interface IDistinctionService
    {
        Task<IEnumerable<DistinctionDTO>> GetAllDistinctionAsync();
        Task<DistinctionDTO> GetDistinctionAsync(int id);
        Task AddDistinction(DistinctionDTO distinctionDTO, ClaimsPrincipal user);
        Task ChangeDistinction(DistinctionDTO distinctionDTO, ClaimsPrincipal user);
        Task DeleteDistinction(int id, ClaimsPrincipal user);
    }
}
