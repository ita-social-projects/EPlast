using EPlast.BussinessLayer.DTO;
using System.Collections.Generic;

namespace EPlast.BussinessLayer.Services.Interfaces
{
    public interface IWorkService
    {
        IEnumerable<WorkDTO> GetAllGroupByPlace();
        IEnumerable<WorkDTO> GetAllGroupByPosition();
    }
}
