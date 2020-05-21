using EPlast.BussinessLayer.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BussinessLayer.Services.Interfaces
{
    public interface IWorkService
    {
        Task<IEnumerable<WorkDTO>> GetAllGroupByPlace();
        Task<IEnumerable<WorkDTO>> GetAllGroupByPosition();
    }
}
