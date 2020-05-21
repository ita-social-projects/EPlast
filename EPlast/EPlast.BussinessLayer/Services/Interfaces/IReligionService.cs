using EPlast.BussinessLayer.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BussinessLayer.Services.Interfaces
{
    public interface IReligionService
    {
        Task<IEnumerable<ReligionDTO>> GetAll();
    }
}
