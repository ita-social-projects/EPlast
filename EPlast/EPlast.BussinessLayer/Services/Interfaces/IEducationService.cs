using EPlast.BussinessLayer.DTO;
using System.Collections.Generic;

namespace EPlast.BussinessLayer.Services.Interfaces
{
    public interface IEducationService
    {
        IEnumerable<EducationDTO> GetAllGroupByPlace();
        IEnumerable<EducationDTO> GetAllGroupBySpeciality();
    }
}
