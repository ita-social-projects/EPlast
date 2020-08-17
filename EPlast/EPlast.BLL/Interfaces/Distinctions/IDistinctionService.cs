using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL
{
    interface IDistinctionService
    {
        Task<IEnumerable<DistinctionDTO>> GetAllDistinctionAsync();
        Task<DistinctionDTO> GetDistinctionAsync(int id);
        DistinctionDTO AddDistinction();
        Task<bool> ChangeDistinction(DistinctionDTO distinctionDTO);
        Task<bool> DeleteDistinction(int id);
    }
}
