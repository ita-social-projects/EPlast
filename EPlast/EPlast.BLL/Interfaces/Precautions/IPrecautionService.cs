using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.DataAccess.Entities;

namespace EPlast.BLL
{
   public interface IPrecautionService
    {
        Task<IEnumerable<PrecautionDTO>> GetAllPrecautionAsync();
        Task<PrecautionDTO> GetPrecautionAsync(int id);
        Task AddPrecautionAsync(PrecautionDTO precautionDTO, User user);
        Task ChangePrecautionAsync(PrecautionDTO precautionDTO, User user);
        Task DeletePrecautionAsync(int id, User user);
    }
}
