using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.BLL.DTO.UserProfiles;

namespace EPlast.BLL.Interfaces.UserProfiles
{
    public interface IUserPersonalDataService
    {
        /// <summary>
        /// Get all education degrees
        /// </summary>
        /// <returns>All education degrees</returns>
        public Task<IEnumerable<DegreeDto>> GetAllDegreesAsync();

        /// <summary>
        /// Get all education and group by place of study
        /// </summary>
        /// <returns>All education</returns>
        public Task<IEnumerable<EducationDto>> GetAllEducationsGroupByPlaceAsync();

        /// <summary>
        /// Get all education and group by speciality
        /// </summary>
        /// <returns>All education</returns>
        public Task<IEnumerable<EducationDto>> GetAllEducationsGroupBySpecialityAsync();

        /// <summary>
        /// Get education
        /// </summary>
        /// <param name="educationId">The id of the education</param>
        /// <returns>Selected education</returns>
        public Task<EducationDto> GetEducationsByIdAsync(int? educationId);

        /// <summary>
        /// Get all genders
        /// </summary>
        /// <returns>All genders</returns>
        public Task<IEnumerable<GenderDto>> GetAllGendersAsync();

        /// <summary>
        /// Get all UPU Degrees
        /// </summary>
        /// <returns>All UPU Degrees</returns>
        public Task<IEnumerable<UpuDegreeDto>> GetAllUpuDegreesAsync();

        /// <summary>
        /// Get all nationalities
        /// </summary>
        /// <returns>All nationalities</returns>
        public Task<IEnumerable<NationalityDto>> GetAllNationalityAsync();

        /// <summary>
        /// Get all religions
        /// </summary>
        /// <returns>All religions</returns>
        public Task<IEnumerable<ReligionDto>> GetAllReligionsAsync();

        /// <summary>
        /// Get all work and group by place of work
        /// </summary>
        /// <returns>All works</returns>
        public Task<IEnumerable<WorkDto>> GetAllWorkGroupByPlaceAsync();

        /// <summary>
        /// Get all work and group by position
        /// </summary>
        /// <returns>All works</returns>
        public Task<IEnumerable<WorkDto>> GetAllWorkGroupByPositionAsync();

        /// <summary>
        /// Get work
        /// </summary>
        /// <param name="workId">The id of the work</param>
        /// <returns>Selected work</returns>
        public Task<WorkDto> GetWorkByIdAsync(int? workId);
    }
}
