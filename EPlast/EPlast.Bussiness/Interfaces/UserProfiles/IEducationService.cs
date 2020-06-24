using EPlast.BusinessLogicLayer.DTO.UserProfiles;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BusinessLogicLayer.Interfaces.UserProfiles
{
    public interface IEducationService
    {
        Task<EducationDTO> GetByIdAsync(int? educationId);
        Task<IEnumerable<EducationDTO>> GetAllGroupByPlaceAsync();
        Task<IEnumerable<EducationDTO>> GetAllGroupBySpecialityAsync();
    }
}
