using EPlast.BLL.DTO.UserProfiles;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.UserProfiles
{
    public interface IDegreeService
    {
        /// <summary>
        /// Get all education degrees
        /// </summary>
        /// <returns>All education degrees</returns>
        Task<IEnumerable<DegreeDTO>> GetAllAsync();
    }
}
