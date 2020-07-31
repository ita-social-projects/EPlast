using EPlast.BLL.DTO.UserProfiles;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.UserProfiles
{
    public interface IGenderService
    {
        /// <summary>
        /// Get all genders
        /// </summary>
        /// <returns>All genders</returns>
        Task<IEnumerable<GenderDTO>> GetAllAsync();
    }
}
