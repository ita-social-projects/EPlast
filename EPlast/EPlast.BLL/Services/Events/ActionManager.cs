using AutoMapper;
using EPlast.BLL.DTO.Events;
using EPlast.BLL.DTO.EventUser;
using EPlast.BLL.Interfaces.Events;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.Event;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.Events
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

        public ActionManager(UserManager<User> userManager, IRepositoryWrapper repoWrapper, IMapper mapper,
            IEventCategoryManager eventCategoryManager, IEventTypeManager eventTypeManager,
            IEventStatusManager eventStatusManager, IParticipantStatusManager participantStatusManager,
            IParticipantManager participantManager, IEventGalleryManager eventGalleryManager)
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

        /// <inheritdoc />
        public async Task<IEnumerable<EventTypeDTO>> GetEventTypesAsync()
        {
            var dto = await _eventTypeManager.GetDTOAsync();
            return dto;
        }

        public async Task<IEnumerable<EventCategoryDTO>> GetActionCategoriesAsync()
        {
            var dto = await _eventCategoryManager.GetDTOAsync();
            return dto;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<EventCategoryDTO>> GetCategoriesByTypeIdAsync(int eventTypeId)
        {
            var dto = await _eventCategoryManager.GetDTOByEventTypeIdAsync(eventTypeId);

            return dto;
        }

        /// <inheritdoc />
        public async Task<List<GeneralEventDTO>> GetEventsAsync(int categoryId, int eventTypeId, ClaimsPrincipal user)
        {
            int approvedStatus = await _participantStatusManager.GetStatusIdAsync("Учасник");
            int undeterminedStatus = await _participantStatusManager.GetStatusIdAsync("Розглядається");
            int rejectedStatus = await _participantStatusManager.GetStatusIdAsync("Відмовлено");
            int approvedEvent = await _eventStatusManager.GetStatusIdAsync("Затверджений(-на)");
            int finishedEvent = await _eventStatusManager.GetStatusIdAsync("Завершений(-на)");
            int notApprovedEvent = await _eventStatusManager.GetStatusIdAsync("Не затверджені");
            await CheckEventsStatusesAsync(categoryId, eventTypeId, finishedEvent);

            var events = await _repoWrapper.Event
                .GetAllAsync(
                    e => e.EventCategoryID == categoryId && e.EventTypeID == eventTypeId,
                    source => source
                        .Include(e => e.EventAdministrations)
                        .Include(e => e.Participants)
                );

            var dto = events
                .Select(ev => new GeneralEventDTO
                {
                    EventId = ev.ID,
                    EventName = ev.EventName,
                    IsUserEventAdmin = (ev.EventAdministrations.Any(e => e.UserID == _userManager.GetUserId(user))) || user.IsInRole("Адміністратор подій"),
                    IsUserParticipant = ev.Participants.Any(p => p.UserId == _userManager.GetUserId(user)),
                    IsUserApprovedParticipant = ev.Participants.Any(p => p.UserId == _userManager.GetUserId(user) && p.ParticipantStatusId == approvedStatus),
                    IsUserUndeterminedParticipant = ev.Participants.Any(p => p.UserId == _userManager.GetUserId(user) && p.ParticipantStatusId == undeterminedStatus),
                    IsUserRejectedParticipant = ev.Participants.Any(p => p.UserId == _userManager.GetUserId(user) && p.ParticipantStatusId == rejectedStatus),
                    IsEventApproved = ev.EventStatusID == approvedEvent,
                    IsEventNotApproved = ev.EventStatusID == notApprovedEvent,
                    IsEventFinished = ev.EventStatusID == finishedEvent
                })
                .ToList();

            return dto;
        }

        /// <inheritdoc />
        public async Task<EventDTO> GetEventInfoAsync(int id, ClaimsPrincipal user)
        {
            int approvedStatus = await _participantStatusManager.GetStatusIdAsync("Учасник");
            int undeterminedStatus = await _participantStatusManager.GetStatusIdAsync("Розглядається");
            int rejectedStatus = await _participantStatusManager.GetStatusIdAsync("Відмовлено");
            int finishedEvent = await _eventStatusManager.GetStatusIdAsync("Завершений(-на)");
            bool isUserGlobalEventAdmin = user?.IsInRole("Адміністратор подій") ?? false;
            await CheckEventStatusAsync(id, finishedEvent);

            var targetEvent = await _repoWrapper.Event
                .GetFirstAsync(
                    e => e.ID == id,
                    source => source
                        .Include(e => e.Participants)
                        .ThenInclude(p => p.User)
                        .Include(e => e.Participants)
                            .ThenInclude(p => p.ParticipantStatus)
                        .Include(e => e.EventStatus)
                        .Include(e => e.EventAdministrations)
                            .ThenInclude(a => a.User)
                        .Include(e => e.EventType)
                        .Include(e => e.EventCategory)
                    );

            var dto = new EventDTO()
            {
                Event = _mapper.Map<Event, EventInfoDTO>(targetEvent),
                IsUserEventAdmin = (targetEvent.EventAdministrations.Any(evAdm => evAdm.UserID == _userManager.GetUserId(user))) || isUserGlobalEventAdmin,
                IsUserParticipant = targetEvent.Participants.Any(p => p.UserId == _userManager.GetUserId(user)),
                IsUserApprovedParticipant = targetEvent.Participants.Any(p => p.UserId == _userManager.GetUserId(user) && p.ParticipantStatusId == approvedStatus),
                IsUserUndeterminedParticipant = targetEvent.Participants.Any(p => p.UserId == _userManager.GetUserId(user) && p.ParticipantStatusId == undeterminedStatus),
                IsUserRejectedParticipant = targetEvent.Participants.Any(p => p.UserId == _userManager.GetUserId(user) && p.ParticipantStatusId == rejectedStatus),
                IsEventFinished = targetEvent.EventStatusID == finishedEvent
            };

            if (!dto.IsUserEventAdmin)
            {
                dto.Event.EventParticipants = dto.Event.EventParticipants.Where(p => p.StatusId == approvedStatus);
            }

            return dto;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<EventGalleryDTO>> GetPicturesAsync(int id)
        {
            var dto = await _eventGalleryManager.GetPicturesInBase64(id);

            return dto;
        }

        /// <inheritdoc />
        public async Task<int> DeleteEventAsync(int id)
        {
            try
            {
                Event objectToDelete = await _repoWrapper.Event
                    .GetFirstAsync(e => e.ID == id);
                _repoWrapper.Event.Delete(objectToDelete);
                await _repoWrapper.SaveAsync();
                return StatusCodes.Status200OK;
            }
            catch
            {
                return StatusCodes.Status400BadRequest;
            }
        }

        /// <inheritdoc />
        public async Task<int> SubscribeOnEventAsync(int id, ClaimsPrincipal user)
        {
            try
            {
                Event targetEvent = await _repoWrapper.Event
                    .GetFirstAsync(e => e.ID == id);
                var userId = _userManager.GetUserId(user);
                int result = await _participantManager.SubscribeOnEventAsync(targetEvent, userId);
                return result;
            }
            catch
            {
                return StatusCodes.Status400BadRequest;
            }
        }

        /// <inheritdoc />
        public async Task<int> UnSubscribeOnEventAsync(int id, ClaimsPrincipal user)
        {
            try
            {
                Event targetEvent = await _repoWrapper.Event
                    .GetFirstAsync(e => e.ID == id);
                var userId = _userManager.GetUserId(user);
                int result = await _participantManager.UnSubscribeOnEventAsync(targetEvent, userId);
                return result;
            }
            catch
            {
                return StatusCodes.Status400BadRequest;
            }
        }

        /// <inheritdoc />
        public async Task<int> ApproveParticipantAsync(int id)
        {
            int result = await _participantManager.ChangeStatusToApprovedAsync(id);
            return result;
        }

        /// <inheritdoc />
        public async Task<int> UnderReviewParticipantAsync(int id)
        {
            int result = await _participantManager.ChangeStatusToUnderReviewAsync(id);
            return result;
        }

        /// <inheritdoc />
        public async Task<int> RejectParticipantAsync(int id)
        {
            int result = await _participantManager.ChangeStatusToRejectedAsync(id);
            return result;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<EventGalleryDTO>> FillEventGalleryAsync(int id, IList<IFormFile> files)
        {
            var uploadedPictures = await _eventGalleryManager.AddPicturesAsync(id, files);
            return uploadedPictures;
        }

        /// <inheritdoc />
        public async Task<int> DeletePictureAsync(int id)
        {
            int result = await _eventGalleryManager.DeletePictureAsync(id);
            return result;
        }


        private async Task CheckEventsStatusesAsync(int id, int actionId, int finishedEvent)
        {
            var eventsToCheck = await _repoWrapper.Event
                .GetAllAsync(e => e.EventCategoryID == id && e.EventTypeID == actionId);
            foreach (var eventToCheck in eventsToCheck)
            {
                if (eventToCheck.EventDateEnd.Date <= DateTime.Now.Date && eventToCheck.EventStatusID != finishedEvent)
                {
                    eventToCheck.EventStatusID = finishedEvent;
                    _repoWrapper.Event.Update(eventToCheck);
                }
            }
            await _repoWrapper.SaveAsync();
        }

        private async Task CheckEventStatusAsync(int id, int finishedEvent)
        {
            var eventToCheck = await _repoWrapper.Event
                .GetFirstAsync(e => e.ID == id);
            if (eventToCheck.EventDateEnd.Date <= DateTime.Now.Date && eventToCheck.EventStatusID != finishedEvent)
            {
                eventToCheck.EventStatusID = finishedEvent;
                _repoWrapper.Event.Update(eventToCheck);
                await _repoWrapper.SaveAsync();
            }
        }

    }
}
