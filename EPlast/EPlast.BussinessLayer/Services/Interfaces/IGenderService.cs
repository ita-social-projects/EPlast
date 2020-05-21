using EPlast.BussinessLayer.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BussinessLayer.Services.Interfaces
{
    public interface IGenderService
    {
        Task<IEnumerable<GenderDTO>> GetAll();
    }
}
