using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL
{
    public interface IDistinctionService
    {
        Task<IEnumerable<DistinctionDTO>> GetAllDistinctionAsync();
        Task<DistinctionDTO> GetDistinctionAsync(int id);
        Task AddDistinction(DistinctionDTO distinction);
        Task ChangeDistinction(DistinctionDTO distinctionDTO);
        Task DeleteDistinction(int id);
    }
}
