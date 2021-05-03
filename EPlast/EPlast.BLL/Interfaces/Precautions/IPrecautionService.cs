using EPlast.DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.DataAccess.Entities.UserEntities;

namespace EPlast.BLL
{
    public interface IPrecautionService
    {
        Task<IEnumerable<PrecautionDTO>> GetAllPrecautionAsync();

        Task<PrecautionDTO> GetPrecautionAsync(int id);

        Task AddPrecautionAsync(PrecautionDTO precautionDTO, User user);

        Task ChangePrecautionAsync(PrecautionDTO precautionDTO, User user);

        Task DeletePrecautionAsync(int id, User user);

        /// <summary>
        /// Returns all searched Precautions
        /// </summary>
        /// <param name="searchedData">Search string</param>
        /// <param name="page">Current page</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Searched Precautions</returns>
        IEnumerable<UserPrecautionsTableObject> GetUsersPrecautions(string searchedData, int page, int pageSize);
    }
}
