using EPlast.DataAccess.Entities;
using System.Collections.Generic;
using EPlast.DataAccess.Entities.Event;

namespace EPlast.BussinessLayer.Interfaces.Events
{
    public interface IEventAdminManager
    {
        IEnumerable<EventAdmin> GetEventAdminsByUserId(string userId);
    }
}
