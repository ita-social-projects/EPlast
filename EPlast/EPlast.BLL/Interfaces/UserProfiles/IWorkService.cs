using EPlast.BLL.DTO.UserProfiles;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.UserProfiles
{
    public interface IWorkService
    {
        Task<WorkDTO> GetByIdAsync(int? workId);
        Task<IEnumerable<WorkDTO>> GetAllGroupByPlaceAsync();
        Task<IEnumerable<WorkDTO>> GetAllGroupByPositionAsync();
    }
}
