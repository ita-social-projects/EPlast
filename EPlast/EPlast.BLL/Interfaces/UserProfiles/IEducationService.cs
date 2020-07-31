using EPlast.BLL.DTO.UserProfiles;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.UserProfiles
{
    public interface IEducationService
    {
        /// <summary>
        /// Get education
        /// </summary>
        /// <param name="educationId">The id of the education</param>
        /// <returns>Selected education</returns>
        Task<EducationDTO> GetByIdAsync(int? educationId);

        /// <summary>
        /// Get all education and group by place of study
        /// </summary>
        /// <returns>All education</returns>
        Task<IEnumerable<EducationDTO>> GetAllGroupByPlaceAsync();

        /// <summary>
        /// Get all education and group by speciality
        /// </summary>
        /// <returns>All education</returns>
        Task<IEnumerable<EducationDTO>> GetAllGroupBySpecialityAsync();
    }
}
