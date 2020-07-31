using EPlast.BLL.DTO.UserProfiles;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.UserProfiles
{
    public interface IWorkService
    {
        /// <summary>
        /// Get work
        /// </summary>
        /// <param name="workId">The id of the work</param>
        /// <returns>Selected work</returns>
        Task<WorkDTO> GetByIdAsync(int? workId);

        /// <summary>
        /// Get all work and group by place of work
        /// </summary>
        /// <returns>All works</returns>
        Task<IEnumerable<WorkDTO>> GetAllGroupByPlaceAsync();

        /// <summary>
        /// Get all work and group by position
        /// </summary>
        /// <returns>All works</returns>
        Task<IEnumerable<WorkDTO>> GetAllGroupByPositionAsync();
    }
}
