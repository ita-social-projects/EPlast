using EPlast.BLL.DTO.UserProfiles;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.UserProfiles
{
    public interface IReligionService
    {
        /// <summary>
        /// Get all religions
        /// </summary>
        /// <returns>All religions</returns>
        Task<IEnumerable<ReligionDTO>> GetAllAsync();
    }
}
