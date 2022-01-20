using EPlast.DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.DataAccess.Entities.UserEntities;
using System;
using EPlast.BLL.DTO.PrecautionsDTO;

namespace EPlast.BLL
{
    public interface IPrecautionService
    {
        Task<IEnumerable<PrecautionDTO>> GetAllPrecautionAsync();

        Task<PrecautionDTO> GetPrecautionAsync(int id);

        Task AddPrecautionAsync(PrecautionDTO precautionDTO, User user);

        Task ChangePrecautionAsync(PrecautionDTO precautionDTO, User user);

        Task DeletePrecautionAsync(int id, User user);        
        Task<Tuple<IEnumerable<UserPrecautionsTableObject>, int>> GetUsersPrecautionsForTableAsync(PrecautionTableSettings tableSettings);
    }
}
