using EPlast.Bussiness.DTO.Events;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.Bussiness.Interfaces.Events
{
    public interface IEventCategoryManager
    {
        Task<List<EventCategoryDTO>> GetDTOAsync();
    }
}
