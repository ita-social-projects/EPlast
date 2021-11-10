using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.Club;
using EPlast.BLL.DTO.Events;
using EPlast.BLL.Interfaces.Events;
using EPlast.BLL.Interfaces.EventUser;
using EPlast.BLL.Services.Club.ClubAccess.ClubAccessGetters;
using EPlast.BLL.Settings;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using Microsoft.AspNetCore.Identity;

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
