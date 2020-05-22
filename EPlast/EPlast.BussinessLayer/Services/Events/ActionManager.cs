using AutoMapper;
using EPlast.BussinessLayer.DTO.Events;
using EPlast.BussinessLayer.Interfaces.Events;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace EPlast.BussinessLayer.Services.Events
{
    public class ActionManager : IActionManager
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IEventCategoryManager _eventCategoryManager;
        private readonly IEventTypeManager _eventTypeManager;
        private readonly IEventStatusManager _eventStatusManager;
        private readonly IParticipantStatusManager _participantStatusManager;
        private readonly IParticipantManager _participantManager;
        private readonly IEventGalleryManager _eventGalleryManager;

        public ActionManager(UserManager<User> userManager, IRepositoryWrapper repoWrapper, IMapper mapper, IEventCategoryManager eventCategoryManager, IEventTypeManager eventTypeManager, IEventStatusManager eventStatusManager, IParticipantStatusManager participantStatusManager, IParticipantManager participantManager, IEventGalleryManager eventGalleryManager)
        {
            _userManager = userManager;
            _repoWrapper = repoWrapper;
            _mapper = mapper;
            _eventCategoryManager = eventCategoryManager;
            _eventTypeManager = eventTypeManager;
            _eventStatusManager = eventStatusManager;
            _participantStatusManager = participantStatusManager;
            _participantManager = participantManager;
            _eventGalleryManager = eventGalleryManager;
        }

        public List<EventCategoryDTO> GetActionCategories()
        {
            var dto = _eventCategoryManager.GetDTO();
            return dto;
        }

        public List<GeneralEventDTO> GetEvents(int id, ClaimsPrincipal user)
        {
            int actionId = _eventTypeManager.GetTypeId("Акція");
            int approvedStatus = _participantStatusManager.GetStatusId("Учасник");
            int undeterminedStatus = _participantStatusManager.GetStatusId("Розглядається");
            int rejectedStatus = _participantStatusManager.GetStatusId("Відмовлено");
            int approvedEvent = _eventStatusManager.GetStatusId("Затверджений(-на)");
            int finishedEvent = _eventStatusManager.GetStatusId("Завершений(-на)");
            int notApprovedEvent = _eventStatusManager.GetStatusId("Не затверджені");
            CheckEventsStatuses(id, actionId, finishedEvent);
            List<GeneralEventDTO> dto = _repoWrapper.Event
             .FindByCondition(e => e.EventCategoryID == id && e.EventTypeID == actionId)
             .Include(e => e.EventAdmins)
             .Include(e => e.Participants)
             .Select(ev => new GeneralEventDTO
             {
                 EventId = ev.ID,
                 EventName = ev.EventName,
                 IsUserEventAdmin = (ev.EventAdmins.Any(e => e.UserID == _userManager.GetUserId(user))) || user.IsInRole("Адміністратор подій"),
                 IsUserParticipant = ev.Participants.Any(p => p.UserId == _userManager.GetUserId(user)),
                 IsUserApprovedParticipant = ev.Participants.Any(p => p.UserId == _userManager.GetUserId(user) && p.ParticipantStatusId == approvedStatus),
                 IsUserUndeterminedParticipant = ev.Participants.Any(p => p.UserId == _userManager.GetUserId(user) && p.ParticipantStatusId == undeterminedStatus),
                 IsUserRejectedParticipant = ev.Participants.Any(p => p.UserId == _userManager.GetUserId(user) && p.ParticipantStatusId == rejectedStatus),
                 IsEventApproved = ev.EventStatusID == approvedEvent,
                 IsEventNotApproved = ev.EventStatusID == notApprovedEvent,
                 IsEventFinished = ev.EventStatusID == finishedEvent
             }).ToList();
            return dto;
        }

        public EventDTO GetEventInfo(int id, ClaimsPrincipal user)
        {
            int approvedStatus = _participantStatusManager.GetStatusId("Учасник");
            int undeterminedStatus = _participantStatusManager.GetStatusId("Розглядається");
            int rejectedStatus = _participantStatusManager.GetStatusId("Відмовлено");
            int finishedEvent = _eventStatusManager.GetStatusId("Завершений(-на)");
            bool isUserGlobalEventAdmin = user?.IsInRole("Адміністратор подій") ?? false;
            CheckEventStatus(id, finishedEvent);
            EventDTO dto = _repoWrapper.Event.FindByCondition(e => e.ID == id)
                   .Include(e => e.Participants)
                        .ThenInclude(p => p.User)
                   .Include(e => e.Participants)
                        .ThenInclude(p => p.ParticipantStatus)
                   .Include(e => e.EventAdmins)
                   .ThenInclude(evAdm => evAdm.User)
                   .Include(e => e.EventStatus)
                   .Include(e => e.EventAdministrations)
                   .Include(e => e.EventType)
                   .Include(e => e.EventCategory)
                   .Include(e => e.EventGallarys)
                        .ThenInclude(eg => eg.Gallary)
                   .Select(e => new EventDTO()
                   {
                       Event = _mapper.Map<Event, EventInfoDTO>(e),
                       IsUserEventAdmin = (e.EventAdmins.Any(evAdm => evAdm.UserID == _userManager.GetUserId(user))) || isUserGlobalEventAdmin,
                       IsUserParticipant = e.Participants.Any(p => p.UserId == _userManager.GetUserId(user)),
                       IsUserApprovedParticipant = e.Participants.Any(p => p.UserId == _userManager.GetUserId(user) && p.ParticipantStatusId == approvedStatus),
                       IsUserUndeterminedParticipant = e.Participants.Any(p => p.UserId == _userManager.GetUserId(user) && p.ParticipantStatusId == undeterminedStatus),
                       IsUserRejectedParticipant = e.Participants.Any(p => p.UserId == _userManager.GetUserId(user) && p.ParticipantStatusId == rejectedStatus),
                       IsEventFinished = e.EventStatusID == finishedEvent
                   })
                   .First();

            if (!dto.IsUserEventAdmin)
            {
                dto.Event.EventParticipants = dto.Event.EventParticipants.Where(p => p.StatusId == approvedStatus);
            }

            return dto;
        }

        public int DeleteEvent(int id)
        {
            try
            {
                Event objectToDelete = _repoWrapper.Event.FindByCondition(e => e.ID == id).First();
                _repoWrapper.Event.Delete(objectToDelete);
                _repoWrapper.Save();
                return StatusCodes.Status200OK;
            }
            catch
            {
                return StatusCodes.Status500InternalServerError;
            }
        }

        public int SubscribeOnEvent(int id, ClaimsPrincipal user)
        {
            try
            {
                Event targetEvent = _repoWrapper.Event.FindByCondition(e => e.ID == id).First();
                var userId = _userManager.GetUserId(user);
                int result = _participantManager.SubscribeOnEvent(targetEvent, userId);
                return result;
            }
            catch
            {
                return StatusCodes.Status500InternalServerError;
            }
        }

        public int UnSubscribeOnEvent(int id, ClaimsPrincipal user)
        {
            try
            {
                Event targetEvent = _repoWrapper.Event.FindByCondition(e => e.ID == id).First();
                var userId = _userManager.GetUserId(user);
                int result = _participantManager.UnSubscribeOnEvent(targetEvent, userId);
                return result;
            }
            catch
            {
                return StatusCodes.Status500InternalServerError;
            }
        }

        public int ApproveParticipant(int id)
        {
            int result = _participantManager.ChangeStatusToApproved(id);
            return result;
        }

        public int UnderReviewParticipant(int id)
        {
            int result = _participantManager.ChangeStatusToUnderReview(id);
            return result;
        }

        public int RejectParticipant(int id)
        {
            int result = _participantManager.ChangeStatusToRejected(id);
            return result;
        }

        public int FillEventGallery(int id, IList<IFormFile> files)
        {
            int result = _eventGalleryManager.AddPictures(id, files);
            return result;
        }

        public int DeletePicture(int id)
        {
            int result = _eventGalleryManager.DeletePicture(id);
            return result;
        }


        private void CheckEventsStatuses(int id, int actionId, int finishedEvent)
        {
            var eventsToCheck = _repoWrapper.Event
                .FindByCondition(e => e.EventCategoryID == id && e.EventTypeID == actionId);
            foreach (var eventToCheck in eventsToCheck)
            {
                if (eventToCheck.EventDateEnd.Date <= DateTime.Now.Date && eventToCheck.EventStatusID != finishedEvent)
                {
                    eventToCheck.EventStatusID = finishedEvent;
                    _repoWrapper.Event.Update(eventToCheck);
                }
            }
            _repoWrapper.Save();
        }

        private void CheckEventStatus(int id, int finishedEvent)
        {
            var eventToCheck = _repoWrapper.Event.FindByCondition(e => e.ID == id).First();
            if (eventToCheck.EventDateEnd.Date <= DateTime.Now.Date && eventToCheck.EventStatusID != finishedEvent)
            {
                eventToCheck.EventStatusID = finishedEvent;
                _repoWrapper.Event.Update(eventToCheck);
                _repoWrapper.Save();
            }
        }
    }
}
