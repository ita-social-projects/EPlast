using EPlast.BusinessLogicLayer.DTO.UserProfiles;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BusinessLogicLayer.Interfaces.UserProfiles
{
    public interface IWorkService
    {
        Task<WorkDTO> GetByIdAsync(int? workId);
        Task<IEnumerable<WorkDTO>> GetAllGroupByPlaceAsync();
        Task<IEnumerable<WorkDTO>> GetAllGroupByPositionAsync();
    }
}
