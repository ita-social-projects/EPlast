using EPlast.BLL.Interfaces.Events;
using EPlast.DataAccess.Entities.Event;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.Events
{
    public class EventAdministrationManager : IEventAdmininistrationManager
    {
        private readonly IRepositoryWrapper _repoWrapper;

        public EventAdministrationManager(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        public async Task<IEnumerable<EventAdministration>> GetEventAdmininistrationByUserIdAsync(string userId)
        {
            var eventAdmins = (await _repoWrapper.EventAdministration.
                GetAllAsync(predicate: i => i.UserID == userId, include: source => source.
                Include(i => i.Event).Include(i => i.User))).ToList();

            return eventAdmins;
        }
    }
}
