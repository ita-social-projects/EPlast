using EPlast.BussinessLayer.DTO;
using System.Collections.Generic;

namespace EPlast.BussinessLayer.Services.Interfaces
{
    public interface IDegreeService
    {
       IEnumerable<DegreeDTO> GetAll();
    }
}
