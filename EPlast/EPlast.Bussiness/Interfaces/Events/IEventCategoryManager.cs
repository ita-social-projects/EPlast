using EPlast.BusinessLogicLayer.DTO.Events;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BusinessLogicLayer.Interfaces.Events
{
    public interface IEventCategoryManager
    {
        Task<List<EventCategoryDTO>> GetDTOAsync();
    }
}
