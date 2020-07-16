using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.BLL.DTO.EventUser;
using EPlast.DataAccess.Entities.Event;

namespace EPlast.BLL.Interfaces.Events
{
    public interface IEventTypeManager
    {
        Task<int> GetTypeIdAsync(string typeName);
        Task<IEnumerable<EventTypeDTO>> GetDTOAsync();
        Task<EventType> GetTypeByIdAsync(int id);
    }
}
