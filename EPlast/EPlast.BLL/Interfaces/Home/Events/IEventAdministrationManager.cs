using EPlast.DataAccess.Entities.Event;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.Events
{
    public interface IEventAdmininistrationManager
    {
        Task<IEnumerable<EventAdministration>> GetEventAdmininistrationByUserIdAsync(string userId);
    }
}
