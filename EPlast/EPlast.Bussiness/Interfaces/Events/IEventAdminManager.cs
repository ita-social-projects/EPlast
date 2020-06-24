﻿using EPlast.DataAccess.Entities.Event;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BusinessLogicLayer.Interfaces.Events
{
    public interface IEventAdminManager
    {
        Task<IEnumerable<EventAdmin>> GetEventAdminsByUserIdAsync(string userId);
    }
}
