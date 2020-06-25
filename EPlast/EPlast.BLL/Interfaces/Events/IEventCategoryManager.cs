using EPlast.BLL.DTO.Events;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.Events
{
    public interface IEventCategoryManager
    {
        Task<List<EventCategoryDTO>> GetDTOAsync();
    }
}
