using EPlast.DataAccess.Entities.Event;
using System.Collections.Generic;

namespace EPlast.BussinessLayer.Interfaces.Events
{
    public interface IEventAdminManager
    {
        IEnumerable<EventAdmin> GetEventAdminsByUserId(string userId);
    }
}
