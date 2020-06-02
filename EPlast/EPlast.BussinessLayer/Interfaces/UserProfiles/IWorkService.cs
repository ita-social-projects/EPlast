using EPlast.BussinessLayer.DTO.UserProfiles;
using System.Collections.Generic;

namespace EPlast.BussinessLayer.Interfaces.UserProfiles
{
    public interface IWorkService
    {
        WorkDTO GetById(int? workId);
        IEnumerable<WorkDTO> GetAllGroupByPlace();
        IEnumerable<WorkDTO> GetAllGroupByPosition();
    }
}
