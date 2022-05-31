using System.Collections.Generic;
using EPlast.BLL.Interfaces.Events;
using EPlast.BLL.Interfaces.EventUser;
using EPlast.DataAccess.Entities;
using System.Linq;
using System.Threading.Tasks;
using EPlast.Resources;
using Microsoft.AspNetCore.Identity;

namespace EPlast.BLL.Services.EventUser.EventUserAccess
{
    public class EventUserAccessService : IEventUserAccessService
    {
        private readonly IEventAdmininistrationManager _eventAdmininistrationManager;
        private readonly UserManager<User> _userManager;
        private readonly IActionManager _actionManager;

        public EventUserAccessService(IEventAdmininistrationManager eventAdmininistrationManager, UserManager<User> userManager, IActionManager actionManager)
        {
            _eventAdmininistrationManager = eventAdmininistrationManager;
            _userManager = userManager;
            _actionManager = actionManager;
        }

        public async Task<bool> IsUserAdminOfEvent(User user, int eventId)
        {
            var eventAdmins = await _eventAdmininistrationManager.GetEventAdmininistrationByUserIdAsync(user.Id);
            return eventAdmins.Any(e => e.EventID == eventId);
        }

        public async Task<string> GetEventStatusAsync(User user, int eventId)
        {
            var eventDetails = await _actionManager.GetEventInfoAsync(eventId, user);
            return eventDetails.Event.EventStatus;
        }

        public async Task<Dictionary<string, bool>> RedefineAccessesAsync(Dictionary<string, bool> userAccesses, User user, int? eventId = null)
        {
            if (eventId == null) return userAccesses;
            
            bool access = await IsUserAdminOfEvent(user, (int)eventId);
            var roles = await _userManager.GetRolesAsync(user);
            var eventStatus = await GetEventStatusAsync(user, (int)eventId);

            userAccesses["SubscribeOnEvent"] = !access;

            if (!(roles.Contains(Roles.Admin) || roles.Contains(Roles.GoverningBodyHead) || roles.Contains(Roles.GoverningBodyAdmin)))
            {
                FunctionalityWithSpecificAccessForEvents.canWhenUserIsAdmin.ForEach(i => userAccesses[i] = access);
                if (eventStatus == "Затверджено")
                {
                    FunctionalityWithSpecificAccessForEvents.cannotWhenEventIsApproved.ForEach(i => userAccesses[i] = false);
                }
            }
            else if (eventStatus == "Завершено")
            {
                userAccesses["EditEvent"] = false;
            }

            return userAccesses;
        }
    }
}
