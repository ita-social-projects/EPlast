using EPlast.BussinessLayer.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EPlast.BussinessLayer.Services.Interfaces
{
    public interface IDegreeService
    {
        Task<IEnumerable<DegreeDTO>> GetAll();
    }
}
