using EPlast.DataAccess.Entities;
using System.Collections.Generic;

namespace EPlast.BussinessLayer.Interfaces.Events
{
    public interface IEventAdminManager
    {
        IEnumerable<EventAdmin> GetEventAdminsByUserId(string userId);
    }
}
