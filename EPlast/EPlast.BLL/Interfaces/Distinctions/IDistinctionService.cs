using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.UserEntities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL
{
    public interface IDistinctionService
    {
        Task<IEnumerable<DistinctionDTO>> GetAllDistinctionAsync();

        Task<DistinctionDTO> GetDistinctionAsync(int id);

        Task AddDistinctionAsync(DistinctionDTO distinctionDTO, User user);

        Task ChangeDistinctionAsync(DistinctionDTO distinctionDTO, User user);

        Task DeleteDistinctionAsync(int id, User user);

        /// <summary>
        /// Returns all searched Distinctions
        /// </summary>
        /// <param name="searchedData">Search string</param>
        /// <param name="page">Current page</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Searched Distinctions</returns>
        IEnumerable<UserDistinctionsTableObject> GetUsersDistinctionsForTable(string searchedData, int page, int pageSize);
    }
}
