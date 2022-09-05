using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPlast.BLL.DTO.Events;
using EPlast.BLL.Interfaces.Events;
using EPlast.BLL.Interfaces.EventUser;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.Event;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using Microsoft.AspNetCore.Identity;

namespace EPlast.BLL.Services.EventUser.EventUserAccess
{
    public class EventUserAccessService : IEventUserAccessService
    {
        private readonly IEventAdmininistrationManager _eventAdmininistrationManager;
        private readonly UserManager<User> _userManager;
        private readonly IActionManager _actionManager;
        private readonly IRepositoryWrapper _repository;

        public EventUserAccessService(
            IEventAdmininistrationManager eventAdmininistrationManager,
            UserManager<User> userManager,
            IActionManager actionManager,
            IRepositoryWrapper repository)
        {
            _eventAdmininistrationManager = eventAdmininistrationManager;
            _userManager = userManager;
            _actionManager = actionManager;
            _repository = repository;
        }

        public async Task<bool> IsUserAdminOfEvent(User user, int eventId)
        {
            var eventAdmins = await _eventAdmininistrationManager.GetEventAdmininistrationByUserIdAsync(user.Id);
            return eventAdmins.Any(e => e.EventID == eventId);
        }

        public async Task<bool> CanPostFeedback(Participant participant, int eventId)
        {
            if (participant == null) return false;
            if (!participant.WasPresent) return false;

            var evt = await _repository.Event.GetFirstOrDefaultAsync(e => e.ID == eventId);

            bool isAppropriatePeriod = DateTime.Now > evt.EventDateEnd && DateTime.Now < evt.EventDateEnd + TimeSpan.FromDays(3);
            if (!isAppropriatePeriod) return false;

            return true;
        }

        public async Task<bool> CanDeleteFeedback(User user, EventFeedback feedback)
        {
            if (feedback.Participant.UserId != user.Id) return false;

            var userRoles = await _userManager.GetRolesAsync(user);
            if (!userRoles.Contains(Roles.Admin)) return false;

            return true;
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
                FunctionalityWithSpecificAccessForEvents.CanWhenUserIsAdmin.ForEach(i => userAccesses[i] = access);
                if (eventStatus == "Затверджено")
                {
                    FunctionalityWithSpecificAccessForEvents.CannotWhenEventIsApproved.ForEach(i => userAccesses[i] = false);
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
