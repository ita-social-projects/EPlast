using EPlast.BLL.DTO.UserProfiles;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.UserProfiles
{
    public interface INationalityService
    {
        /// <summary>
        /// Get all nationalities
        /// </summary>
        /// <returns>All nationalities</returns>
        Task<IEnumerable<NationalityDTO>> GetAllAsync();
    }
}
