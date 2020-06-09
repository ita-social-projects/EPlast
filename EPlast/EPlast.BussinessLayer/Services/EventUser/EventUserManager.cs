using AutoMapper;
using EPlast.BussinessLayer.DTO.EventUser;
using EPlast.BussinessLayer.DTO.UserProfiles;
using EPlast.BussinessLayer.Interfaces.Events;
using EPlast.BussinessLayer.Interfaces.EventUser;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.Event;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

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

        public async Task<EventUserDTO> EventUserAsync(string userId, ClaimsPrincipal user)
        {
            var currentUserId = _userManager.GetUserId(user);
            if (string.IsNullOrEmpty(userId))
            {
                userId = currentUserId;
            }

            var targetUser = await _repoWrapper.User.FindByCondition(q => q.Id == userId).FirstAsync();
            EventUserDTO model = new EventUserDTO { User = _mapper.Map<User, UserDTO>(targetUser) };
            var eventAdmins = await _eventAdminManager.GetEventAdminsByUserIdAsync(userId);
            var participants = await _participantManager.GetParticipantsByUserIdAsync(userId);
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

        public async Task<EventCreateDTO> InitializeEventCreateDTOAsync()
        {
            var eventCategories =await _eventCategoryManager.GetDTOAsync();
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

        public async Task<EventCreateDTO> InitializeEventCreateDTOAsync(int eventId)
        {
            var createdEvent = await _repoWrapper.Event.FindByCondition(e => e.ID == eventId)
                .Include(e => e.EventAdmins)
                .FirstAsync();
            var userId = createdEvent.EventAdmins.First().UserID;
            var setAdministrationModel = new EventCreateDTO()
            {
                Event = _mapper.Map<Event, EventCreationDTO>(createdEvent),
                Users = _mapper.Map<List<User>, IEnumerable<UserDTO>>(_repoWrapper.User.FindByCondition(i => i.Id != userId).ToList())
            };
            return setAdministrationModel;
        }

        public async Task<int> CreateEventAsync(EventCreateDTO model)
        {
            model.Event.EventStatusID = _eventStatusManager.GetStatusId("Не затверджені");
            var eventToCreate = _mapper.Map<EventCreationDTO, Event>(model.Event);
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
            await _repoWrapper.EventAdmin.CreateAsync(eventAdmin);
            await _repoWrapper.EventAdministration.CreateAsync(eventAdministration);
            await _repoWrapper.Event.CreateAsync(eventToCreate);
            await _repoWrapper.SaveAsync();
            return eventToCreate.ID;
        }

        public async Task SetAdministrationAsync(EventCreateDTO model)
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
            await _repoWrapper.EventAdmin.CreateAsync(eventAdmin);
            await _repoWrapper.EventAdministration.CreateAsync(eventAdministration);
            await _repoWrapper.SaveAsync();
        }

        public async Task<EventCreateDTO> InitializeEventEditDTOAsync(int eventId)
        {
            var editedEvent = await _repoWrapper.Event.
                FindByCondition(e => e.ID == eventId).
                Include(q => q.EventCategory).
                Include(q => q.EventAdmins).
                ThenInclude(q => q.User).
                Include(q => q.EventAdministrations).
                ThenInclude(q => q.User).
                Include(q => q.EventType).
                Include(q => q.EventStatus).
                Include(q => q.Participants).
                FirstAsync();

            var users = _mapper.Map<List<User>, IEnumerable<UserDTO>>(_repoWrapper.User.FindAll().ToList());
            var eventTypes = _mapper.Map<List<EventType>, IEnumerable<EventTypeDTO>>(_repoWrapper.EventType.FindAll().ToList());
            var eventCategories = await _eventCategoryManager.GetDTOAsync();
            var model = new EventCreateDTO()
            {
                Event = _mapper.Map<Event, EventCreationDTO>(editedEvent),
                Users = users,
                EventTypes = eventTypes,
                EventCategories = eventCategories,
            };
            return model;
        }

        public async Task EditEventAsync(EventCreateDTO model)
        {
            var eventEdit = _mapper.Map<EventCreationDTO, Event>(model.Event);
            _repoWrapper.Event.Update(eventEdit);
            await _repoWrapper.SaveAsync();
        }
    }
}