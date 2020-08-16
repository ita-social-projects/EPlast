using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.Distinctions
{
    interface IDistinctionService
    {
        Task<IEnumerable<DistinctionDTO>> GetAllDistinctionAsync();
        Task<DistinctionDTO> GetDistinctionAsync(int id);
        DistinctionDTO AddDistinction();
        Task<DistinctionDTO> ChangeDistinction(DistinctionDTO distinctionDTO);
        Task<bool> DeleteDistinction(int id);
    }
}
