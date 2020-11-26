using EPlast.BLL.DTO.UserProfiles;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.UserProfiles
{
    public interface IUserPersonalDataService
    {
        /// <summary>
        /// Get all education degrees
        /// </summary>
        /// <returns>All education degrees</returns>
        public Task<IEnumerable<DegreeDTO>> GetAllDegreesAsync();

        /// <summary>
        /// Get all education and group by place of study
        /// </summary>
        /// <returns>All education</returns>
        public Task<IEnumerable<EducationDTO>> GetAllEducationsGroupByPlaceAsync();

        /// <summary>
        /// Get all education and group by speciality
        /// </summary>
        /// <returns>All education</returns>
        public Task<IEnumerable<EducationDTO>> GetAllEducationsGroupBySpecialityAsync();

        /// <summary>
        /// Get education
        /// </summary>
        /// <param name="educationId">The id of the education</param>
        /// <returns>Selected education</returns>
        public Task<EducationDTO> GetEducationsByIdAsync(int? educationId);

        /// <summary>
        /// Get all genders
        /// </summary>
        /// <returns>All genders</returns>
        public Task<IEnumerable<GenderDTO>> GetAllGendersAsync();

        /// <summary>
        /// Get all nationalities
        /// </summary>
        /// <returns>All nationalities</returns>
        public Task<IEnumerable<NationalityDTO>> GetAllNationalityAsync();

        /// <summary>
        /// Get all religions
        /// </summary>
        /// <returns>All religions</returns>
        public Task<IEnumerable<ReligionDTO>> GetAllReligionsAsync();

        /// <summary>
        /// Get all work and group by place of work
        /// </summary>
        /// <returns>All works</returns>
        public Task<IEnumerable<WorkDTO>> GetAllWorkGroupByPlaceAsync();

        /// <summary>
        /// Get all work and group by position
        /// </summary>
        /// <returns>All works</returns>
        public Task<IEnumerable<WorkDTO>> GetAllWorkGroupByPositionAsync();

        /// <summary>
        /// Get work
        /// </summary>
        /// <param name="workId">The id of the work</param>
        /// <returns>Selected work</returns>
        public Task<WorkDTO> GetWorkByIdAsync(int? workId);
    }
}
