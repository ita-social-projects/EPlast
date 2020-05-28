using AutoMapper;
using EPlast.BussinessLayer.DTO.EventUser;
using EPlast.BussinessLayer.Interfaces.Events;
using EPlast.BussinessLayer.Interfaces.EventUser;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using EPlast.BussinessLayer.DTO;
using EPlast.BussinessLayer.DTO.UserProfiles;
using Microsoft.EntityFrameworkCore;

namespace EPlast.BussinessLayer.Services.EventUser
{
    public class EventUserManager : IEventUserManager
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IParticipantStatusManager _participantStatusManager;
        private readonly IParticipantManager _participantManager;
        private readonly IEventAdminManager _eventAdminManager;
        private readonly IEventCategoryManager _eventCategoryManager;
        private readonly IEventStatusManager _eventStatusManager;

        public EventUserManager(IRepositoryWrapper repoWrapper, UserManager<User> userManager,
            IParticipantStatusManager participantStatusManager, IMapper mapper, IParticipantManager participantManager,
            IEventAdminManager eventAdminManager, IEventCategoryManager eventCategoryManager,
            IEventStatusManager eventStatusManager)
        {
            _repoWrapper = repoWrapper;
            _userManager = userManager;
            _participantStatusManager = participantStatusManager;
            _mapper = mapper;
            _participantManager = participantManager;
            _eventAdminManager = eventAdminManager;
            _eventCategoryManager = eventCategoryManager;
            _eventStatusManager = eventStatusManager;
        }

        public EventUserDTO EventUser(string userId, ClaimsPrincipal user)
        {
            var currentUserId = _userManager.GetUserId(user);
            if (string.IsNullOrEmpty(userId))
            {
                userId = currentUserId;
            }

            var _user = _repoWrapper.User.FindByCondition(q => q.Id == userId).First();
            EventUserDTO model = new EventUserDTO { User = _mapper.Map<User, UserDTO>(_user) };
            var eventAdmins = _eventAdminManager.GetEventAdminsByUserId(userId);
            var participants = _participantManager.GetParticipantsByUserId(userId);
            model.CreatedEvents = new List<EventGeneralInfoDTO>();
            foreach (var eventAdmin in eventAdmins)
            {
                var eventToAdd = _mapper.Map<Event, EventGeneralInfoDTO>(eventAdmin.Event);
                model.CreatedEvents.Add(eventToAdd);
            }

            model.PlanedEvents = new List<EventGeneralInfoDTO>();
            model.VisitedEvents = new List<EventGeneralInfoDTO>();
            foreach (var participant in participants)
            {
                var eventToAdd = _mapper.Map<Event, EventGeneralInfoDTO>(participant.Event);
                if (participant.Event.EventDateEnd >= DateTime.Now)
                {
                    model.PlanedEvents.Add(eventToAdd);
                }
                else if (participant.Event.EventDateEnd < DateTime.Now &&
                         participant.ParticipantStatusId == _participantStatusManager.GetStatusId("Учасник"))
                {
                    model.VisitedEvents.Add(eventToAdd);
                }
            }
            return model;
        }

        public EventCreateDTO InitializeEventCreateDTO()
        {
            var eventCategories = _eventCategoryManager.GetDTO();
            var users = _mapper.Map<List<User>, IEnumerable<UserDTO>>(_repoWrapper.User.FindAll().ToList());
            var eventTypes = _mapper.Map<List<EventType>, IEnumerable<EventTypeDTO>>(_repoWrapper.EventType.FindAll().ToList());
            var model = new EventCreateDTO()
            {
                Users = users,
                EventTypes = eventTypes,
                EventCategories = eventCategories
            };
            return model;
        }

        public EventCreateDTO InitializeEventCreateDTO(int eventId)
        {
            var createdEvent = _repoWrapper.Event.FindByCondition(e => e.ID == eventId)
                .Include(e => e.EventAdmins)
                .First();
            var userId = createdEvent.EventAdmins.First().UserID;
            var setAdministrationModel = new EventCreateDTO()
            {
                Event = _mapper.Map<Event, EventDTO>(createdEvent),
                Users = _mapper.Map<List<User>, IEnumerable<UserDTO>>(_repoWrapper.User.FindByCondition(i => i.Id != userId).ToList())
            };
            return setAdministrationModel;
        }

        public int CreateEvent(EventCreateDTO model)
        {
            model.Event.EventStatusID = _eventStatusManager.GetStatusId("Не затверджені");
            var eventToCreate = _mapper.Map<EventDTO, Event>(model.Event);
            EventAdmin eventAdmin = new EventAdmin()
            {
                Event = eventToCreate,
                UserID = model.EventAdmin.UserID
            };
            EventAdministration eventAdministration = new EventAdministration()
            {
                Event = eventToCreate,
                AdministrationType = "Бунчужний/на",
                UserID = model.EventAdministration.UserID
            };
            _repoWrapper.EventAdmin.Create(eventAdmin);
            _repoWrapper.EventAdministration.Create(eventAdministration);
            _repoWrapper.Event.Create(eventToCreate);
            _repoWrapper.Save();
            return eventToCreate.ID;
        }

        public void SetAdministration(EventCreateDTO model)
        {
            EventAdmin eventAdmin = new EventAdmin()
            {
                EventID = model.Event.ID,
                UserID = model.EventAdmin.UserID
            };
            EventAdministration eventAdministration = new EventAdministration()
            {
                EventID = model.Event.ID,
                AdministrationType = "Писар",
                UserID = model.EventAdministration.UserID
            };
            _repoWrapper.EventAdmin.Create(eventAdmin);
            _repoWrapper.EventAdministration.Create(eventAdministration);
            _repoWrapper.Save();
        }
    }
}