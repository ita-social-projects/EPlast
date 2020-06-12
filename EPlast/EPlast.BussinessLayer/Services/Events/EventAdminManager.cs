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
            var eventAdmins = (await _repoWrapper.EventAdmin.GetAllAsync(predicate: i => i.UserID == userId, include: source => source.
                Include(i => i.Event).Include(i => i.User))).ToList();

            return eventAdmins;
        }
    }
}
