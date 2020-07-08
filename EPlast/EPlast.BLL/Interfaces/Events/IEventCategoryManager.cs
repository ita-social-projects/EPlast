using EPlast.BLL.DTO.Events;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.Events
{
    public interface IEventCategoryManager
    {
        Task<IEnumerable<EventCategoryDTO>> GetDTOAsync();
        Task<IEnumerable<EventCategoryDTO>> GetDTOByEventTypeIdAsync(int eventTypeId);
    }
}
