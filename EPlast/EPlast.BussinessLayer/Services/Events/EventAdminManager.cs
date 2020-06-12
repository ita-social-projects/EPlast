using EPlast.BussinessLayer.Interfaces.Events;
using EPlast.DataAccess.Entities.Event;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.BussinessLayer.Services.Events
{
    public class EventAdminManager : IEventAdminManager
    {
        private readonly IRepositoryWrapper _repoWrapper;

        public EventAdminManager(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        public async Task<IEnumerable<EventAdmin>> GetEventAdminsByUserIdAsync(string userId)
        {
            var eventAdmins =await _repoWrapper.EventAdmin.FindByCondition(i => i.UserID == userId)
                .Include(i => i.Event).Include(i => i.User).ToListAsync();
            return eventAdmins;
        }
    }
}
