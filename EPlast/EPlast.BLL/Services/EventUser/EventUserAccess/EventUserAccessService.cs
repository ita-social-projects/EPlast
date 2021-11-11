using EPlast.BLL.Interfaces.Events;
using EPlast.BLL.Interfaces.EventUser;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.EventUser.EventUserAccess
{
    public class EventUserAccessService : IEventUserAccessService
    {
        private readonly IEventAdmininistrationManager _eventAdmininistrationManager;

        public EventUserAccessService(IEventAdmininistrationManager eventAdmininistrationManager)
        {
            _eventAdmininistrationManager = eventAdmininistrationManager;
        }

        public async Task<bool> HasAccessAsync(DataAccess.Entities.User user, int eventId)
        {
            var eventAdmins = await _eventAdmininistrationManager.GetEventAdmininistrationByUserIdAsync(user.Id);
            return eventAdmins.Any(e => e.EventID == eventId);
        }

    }
}
