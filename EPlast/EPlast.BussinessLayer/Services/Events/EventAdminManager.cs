using EPlast.BussinessLayer.Interfaces.Events;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace EPlast.BussinessLayer.Services.Events
{
    public class EventAdminManager : IEventAdminManager
    {
        private readonly IRepositoryWrapper _repoWrapper;

        public EventAdminManager(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        public IEnumerable<EventAdmin> GetEventAdminsByUserId(string userId)
        {
            var eventAdmins = _repoWrapper.EventAdmin.FindByCondition(i => i.UserID == userId)
                .Include(i => i.Event).Include(i => i.User).ToList();
            return eventAdmins;
        }
    }
}
