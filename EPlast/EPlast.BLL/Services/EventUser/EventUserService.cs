using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.EventUser;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces.Events;
using EPlast.BLL.Interfaces.EventUser;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.Event;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;

namespace EPlast.BLL.Services.EventUser
{
    /// <inheritdoc/>
    public class EventUserService : IEventUserService
    {
        private readonly IRepositoryWrapper repoWrapper;
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;
        private readonly IParticipantStatusManager participantStatusManager;
        private readonly IParticipantManager participantManager;
        private readonly IEventAdmininistrationManager eventAdmininistrationManager;


        public EventUserService(IRepositoryWrapper repoWrapper, UserManager<User> userManager,
            IParticipantStatusManager participantStatusManager, IMapper mapper, IParticipantManager participantManager,
            IEventAdmininistrationManager eventAdmininistrationManager)
        {
            this.repoWrapper = repoWrapper;
            this.userManager = userManager;
            this.participantStatusManager = participantStatusManager;
            this.mapper = mapper;
            this.participantManager = participantManager;
            this.eventAdmininistrationManager = eventAdmininistrationManager;
        }

        public async Task<EventUserDto> EventUserAsync(string userId, User user)
        {
            if (string.IsNullOrEmpty(userId))
            {
                userId = await userManager.GetUserIdAsync(user);
            }

            var userWithRoles = await userManager.FindByIdAsync(userId);
            var model = new EventUserDto
            {
                User = mapper.Map<User, UserDto>(await repoWrapper.User.GetFirstAsync(predicate: q => q.Id == userId)),
                UserRoles = await userManager.GetRolesAsync(userWithRoles)
            };

            var eventAdmins = await eventAdmininistrationManager.GetEventAdmininistrationByUserIdAsync(userId);
            model.CreatedEvents = new List<EventGeneralInfoDto>();
            foreach (var eventAdmin in eventAdmins)
            {
                var eventToAdd = mapper.Map<Event, EventGeneralInfoDto>(eventAdmin.Event);
                if (eventToAdd.EventDateEnd > DateTime.Now)
                {
                    model.CreatedEvents.Add(eventToAdd);
                }
            }

            var participants = await participantManager.GetParticipantsByUserIdAsync(userId);
            model.PlanedEvents = new List<EventGeneralInfoDto>();
            model.VisitedEvents = new List<EventGeneralInfoDto>();
            foreach (var participant in participants)
            {
                var eventToAdd = mapper.Map<Event, EventGeneralInfoDto>(participant.Event);
                if (participant.Event.EventDateEnd >= DateTime.Now)
                {
                    model.PlanedEvents.Add(eventToAdd);
                }
                else if (participant.Event.EventDateEnd < DateTime.Now &&
                         participant.ParticipantStatusId == await participantStatusManager.GetStatusIdAsync("Учасник"))
                {
                    model.VisitedEvents.Add(eventToAdd);
                }
            }
            return model;
        }
    }
}

