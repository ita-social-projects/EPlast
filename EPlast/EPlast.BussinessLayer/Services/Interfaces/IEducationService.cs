using EPlast.BussinessLayer.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BussinessLayer.Services.Interfaces
{
    public interface IEducationService
    {
        Task<IEnumerable<EducationDTO>> GetAllGroupByPlace();
        Task<IEnumerable<EducationDTO>> GetAllGroupBySpeciality();
    }
}
