using EPlast.BussinessLayer.DTO.UserProfiles;
using System.Collections.Generic;

namespace EPlast.BussinessLayer.Interfaces.UserProfiles
{
    public interface IEducationService
    {
        EducationDTO GetById(int? educationId);
        IEnumerable<EducationDTO> GetAllGroupByPlace();
        IEnumerable<EducationDTO> GetAllGroupBySpeciality();
    }
}
