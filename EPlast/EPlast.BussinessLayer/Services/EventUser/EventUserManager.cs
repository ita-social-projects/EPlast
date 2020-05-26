using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using EPlast.BussinessLayer.DTO.EventUser;
using EPlast.BussinessLayer.Interfaces.EventUser;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EPlast.BussinessLayer.Services.EventUser
{
    public class EventUserManager : IEventUserManager
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;


        public EventUserManager(IRepositoryWrapper repoWrapper, UserManager<User> userManager, IMapper mapper)
        {
            _repoWrapper = repoWrapper;
            _userManager = userManager;
            _mapper = mapper;
        }

        public EventUserDTO EventUser(string userId, ClaimsPrincipal user)
        {
            var _currentUserId = _userManager.GetUserId(user);
            if (string.IsNullOrEmpty(userId))
            {
                userId = _currentUserId;
            }

            var _user = _repoWrapper.User.FindByCondition(q => q.Id == userId).First();

            EventUserDTO model = new EventUserDTO();

            model.User = _mapper.Map<User, UserDTO>(_user);
            var eventAdmins = _repoWrapper.EventAdmin.FindByCondition(i => i.UserID == _userManager.GetUserId(user))
                .Include(i => i.Event).Include(i => i.User).ToList();
            var participants = _repoWrapper.Participant.FindByCondition(i => i.UserId == _userManager.GetUserId(user))
                .Include(i => i.Event).ToList();
            model.CreatedEventCount = 0;
            model.CreatedEvents = new List<EventGeneralInfoDTO>();
            foreach (var eventAdmin in eventAdmins)
            {
                if (eventAdmin.UserID == _userManager.GetUserId(user))
                {
                    var eventToAdd = _mapper.Map<Event, EventGeneralInfoDTO>(eventAdmin.Event);
                    model.CreatedEvents.Add(eventToAdd);
                    model.CreatedEventCount += 1;
                }
            }

            model.PlanedEventCount = 0;
            model.PlanedEvents = new List<EventGeneralInfoDTO>();
            model.VisitedEventsCount = 0;
            model.VisitedEvents = new List<EventGeneralInfoDTO>();
            foreach (var participant in participants)
            {
                var eventToAdd = _mapper.Map<Event, EventGeneralInfoDTO>(participant.Event);
                if (participant.UserId == _userManager.GetUserId(user) &&
                    participant.Event.EventDateEnd >= DateTime.Now)
                {
                    model.PlanedEvents.Add(eventToAdd);
                    model.PlanedEventCount += 1;
                }
                else if (participant.UserId == _userManager.GetUserId(user) &&
                         participant.Event.EventDateEnd < DateTime.Now &&
                         participant == _repoWrapper.Participant.FindByCondition(i =>
                             i.ParticipantStatus.ParticipantStatusName == "Учасник"))
                {
                    model.VisitedEventsCount += 1;
                    model.VisitedEvents.Add(eventToAdd);
                }
            }
            return model;
        }
    }
}