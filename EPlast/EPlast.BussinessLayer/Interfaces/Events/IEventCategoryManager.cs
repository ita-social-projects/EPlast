using EPlast.BussinessLayer.DTO.Events;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BussinessLayer.Interfaces.Events
{
    public interface IEventCategoryManager
    {
        Task<List<EventCategoryDTO>> GetDTOAsync();
    }
}
