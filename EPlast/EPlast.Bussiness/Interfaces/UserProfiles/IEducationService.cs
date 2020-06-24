using EPlast.Bussiness.DTO.UserProfiles;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.Bussiness.Interfaces.UserProfiles
{
    public interface IEducationService
    {
        Task<EducationDTO> GetByIdAsync(int? educationId);
        Task<IEnumerable<EducationDTO>> GetAllGroupByPlaceAsync();
        Task<IEnumerable<EducationDTO>> GetAllGroupBySpecialityAsync();
    }
}
