using AutoMapper;
using EPlast.BLL.DTO.Events;
using EPlast.BLL.DTO.EventUser;
using EPlast.BLL.Interfaces.Events;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.Event;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.Events
{
    public class ActionManager : IActionManager
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IParticipantStatusManager _participantStatusManager;
        private readonly IParticipantManager _participantManager;
        private readonly IEventWrapper _eventWrapper;

        public ActionManager(UserManager<User> userManager, IRepositoryWrapper repoWrapper, IMapper mapper,
            IParticipantStatusManager participantStatusManager, IParticipantManager participantManager,
            IEventWrapper eventWrapper)
        {
            _userManager = userManager;
            _repoWrapper = repoWrapper;
            _mapper = mapper;
            _participantStatusManager = participantStatusManager;
            _participantManager = participantManager;
            _eventWrapper = eventWrapper;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<EventTypeDTO>> GetEventTypesAsync()
        {
            return await _eventWrapper.EventTypeManager.GetEventTypesDTOAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<EventCategoryDTO>> GetActionCategoriesAsync()
        {
            return await _eventWrapper.EventCategoryManager.GetDTOAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<EventSectionDTO>> GetEventSectionsAsync()
        {
            return await _eventWrapper.EventSectionManager.GetEventSectionsDTOAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<EventCategoryDTO>> GetCategoriesByTypeIdAsync(int eventTypeId)
        {
            return await _eventWrapper.EventCategoryManager.GetDTOByEventTypeIdAsync(eventTypeId);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<EventCategoryDTO>> GetCategoriesByPageAsync(int eventTypeId, int page, int pageSize, string CategoryName = null)
        {
            return await _eventWrapper.EventCategoryManager.GetDTOByEventPageAsync(eventTypeId, page, pageSize);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<GeneralEventDTO>> GetEventsAsync(int categoryId, int eventTypeId, User user)
        {
            var events = await _repoWrapper.Event
                .GetAllAsync(
                    e => e.EventCategoryID == categoryId,
                    source => source
                        .Include(e => e.EventAdministrations)
                        .Include(e => e.Participants)
                );
            
            return await GetEventDtosAsync(events, user);
        }

        /// <inheritdoc />
        public async Task<EventDTO> GetEventInfoAsync(int id, User user)
        {
            int approvedStatus = await _participantStatusManager.GetStatusIdAsync("Учасник");
            int undeterminedStatus = await _participantStatusManager.GetStatusIdAsync("Розглядається");
            int rejectedStatus = await _participantStatusManager.GetStatusIdAsync("Відмовлено");
            int finishedEvent = await _eventWrapper.EventStatusManager.GetStatusIdAsync("Завершено");
            var userRoles = await _userManager.GetRolesAsync(user);
            bool isUserGlobalEventAdmin = userRoles?.Contains(Roles.EventAdministrator) ?? false;

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
                        .Include(e => e.EventAdministrations)
                        .ThenInclude(a => a.EventAdministrationType)
                        .Include(e => e.EventType)
                        .Include(e => e.EventCategory)
                );

            var dto = new EventDTO()
            {
                Event = _mapper.Map<Event, EventInfoDTO>(targetEvent),
                IsUserEventAdmin =
                    (targetEvent.EventAdministrations.Any(evAdm =>
                        evAdm.UserID == _userManager.GetUserIdAsync(user).Result)) || isUserGlobalEventAdmin,
                IsUserParticipant =
                    targetEvent.Participants.Any(p => p.UserId == _userManager.GetUserIdAsync(user).Result),
                IsUserApprovedParticipant = targetEvent.Participants.Any(p =>
                    p.UserId == _userManager.GetUserIdAsync(user).Result && p.ParticipantStatusId == approvedStatus),
                IsUserUndeterminedParticipant = targetEvent.Participants.Any(p =>
                    p.UserId == _userManager.GetUserIdAsync(user).Result &&
                    p.ParticipantStatusId == undeterminedStatus),
                IsUserRejectedParticipant = targetEvent.Participants.Any(p =>
                    p.UserId == _userManager.GetUserIdAsync(user).Result && p.ParticipantStatusId == rejectedStatus),
                IsEventFinished = targetEvent.EventStatusID == finishedEvent
            };

            if (!dto.IsUserEventAdmin && dto.ParticipantAssessment != 0)
            {
                dto.Event.EventParticipants = dto.Event.EventParticipants.Where(p => p.StatusId == approvedStatus);
            }

            if (dto.IsUserApprovedParticipant
                && dto.IsEventFinished
                && (DateTime.Now < targetEvent.EventDateEnd.Add(new TimeSpan(3, 0, 0, 0))))
            {
                dto.CanEstimate = true;
                dto.ParticipantAssessment = targetEvent.Participants
                    .First(p => p.UserId == _userManager.GetUserIdAsync(user).Result).Estimate;
            }

            return dto;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<EventGalleryDTO>> GetPicturesAsync(int id)
        {
            return await _eventWrapper.EventGalleryManager.GetPicturesInBase64(id);
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
        public async Task<int> SubscribeOnEventAsync(int id, User user)
        {
            try
            {
                Event targetEvent = await _repoWrapper.Event
                    .GetFirstAsync(e => e.ID == id);
                var userId = await _userManager.GetUserIdAsync(user);
                int result = await _participantManager.SubscribeOnEventAsync(targetEvent, userId);
                return result;
            }
            catch
            {
                return StatusCodes.Status400BadRequest;
            }
        }

        /// <inheritdoc />
        public async Task<int> UnSubscribeOnEventAsync(int id, User user)
        {
            try
            {
                Event targetEvent = await _repoWrapper.Event
                    .GetFirstAsync(e => e.ID == id);
                var userId = await _userManager.GetUserIdAsync(user);
                int result = await _participantManager.UnSubscribeOnEventAsync(targetEvent, userId);
                return result;
            }
            catch
            {
                return StatusCodes.Status400BadRequest;
            }
        }

        public async Task<int> EstimateEventAsync(int eventId, User user, double estimate)
        {
            try
            {
                var userId = await _userManager.GetUserIdAsync(user);
                var newRating = await _participantManager.EstimateEventByParticipantAsync(eventId, userId, estimate);
                var targetEvent = await _repoWrapper.Event.GetFirstAsync(e => e.ID == eventId);
                targetEvent.Rating = newRating;
                _repoWrapper.Event.Update(targetEvent);
                await _repoWrapper.SaveAsync();

                return StatusCodes.Status200OK;
            }
            catch
            {
                return StatusCodes.Status400BadRequest;
            }
        }

        /// <inheritdoc />
        public async Task<int> ApproveParticipantAsync(int id)
        {
            return await _participantManager.ChangeStatusToApprovedAsync(id);
        }

        /// <inheritdoc />
        public async Task<int> UnderReviewParticipantAsync(int id)
        {
            return await _participantManager.ChangeStatusToUnderReviewAsync(id);
        }

        /// <inheritdoc />
        public async Task<int> RejectParticipantAsync(int id)
        {
            return await _participantManager.ChangeStatusToRejectedAsync(id);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<EventGalleryDTO>> FillEventGalleryAsync(int id, IList<IFormFile> files)
        {
            return await _eventWrapper.EventGalleryManager.AddPicturesAsync(id, files);
        }

        /// <inheritdoc />
        public async Task<int> DeletePictureAsync(int id)
        {
            return await _eventWrapper.EventGalleryManager.DeletePictureAsync(id);
        }

        public async Task CheckEventsStatusesAsync()
        {
            int finishedEventStatus = 1;
            var eventsToCheck = await _repoWrapper.Event
                .GetAllAsync(e => e.EventStatusID != finishedEventStatus && (DateTime.Compare(e.EventDateEnd, DateTime.Now) < 0));

            foreach (var eventToCheck in eventsToCheck)
            {
                eventToCheck.EventStatusID = finishedEventStatus;
                _repoWrapper.Event.Update(eventToCheck);
            }
            await _repoWrapper.SaveAsync();
        }

        public async Task<IEnumerable<GeneralEventDTO>> GetEventsByStatusAsync(int categoryId, int typeId, int status, User user)
        {
            IEnumerable<Event> events;
            if (status == 1)
            {
                events = await _repoWrapper.Event
                   .GetAllAsync(
                       e => e.EventCategoryID == categoryId && e.EventTypeID == typeId && e.EventStatus.ID == status,
                       source => source
                           .Include(e => e.EventAdministrations)
                           .Include(e => e.Participants)
                   );

            }
            else
            {
                events = await _repoWrapper.Event
                  .GetAllAsync(
                      e => e.EventCategoryID == categoryId && e.EventTypeID == typeId,
                      source => source
                          .Include(e => e.EventAdministrations)
                          .Include(e => e.Participants)
                  );

            }

            var dto = await GetEventDtosAsync(events, user);
            return dto;
        }

        private async Task<List<GeneralEventDTO>> GetEventDtosAsync(IEnumerable<Event> events, User user)
        {
            int approvedStatus = await _participantStatusManager.GetStatusIdAsync("Учасник");
            int undeterminedStatus = await _participantStatusManager.GetStatusIdAsync("Розглядається");
            int rejectedStatus = await _participantStatusManager.GetStatusIdAsync("Відмовлено");
            int approvedEvent = await _eventWrapper.EventStatusManager.GetStatusIdAsync("Затверджено");
            int finishedEvent = await _eventWrapper.EventStatusManager.GetStatusIdAsync("Завершено");
            int notApprovedEvent = await _eventWrapper.EventStatusManager.GetStatusIdAsync("Не затвердженo");
            var userRoles = await _userManager.GetRolesAsync(user);

            var eventAdmins = await _repoWrapper.EventAdministration.GetAllAsync();

            return events
                .Select(ev => new GeneralEventDTO
                {
                    EventId = ev.ID,
                    EventName = ev.EventName,
                    IsUserEventAdmin = ev.EventAdministrations.Any( e => e.UserID == _userManager.GetUserIdAsync(user).Result) || userRoles != null && userRoles.Contains(Roles.EventAdministrator),
                    IsUserParticipant = ev.Participants.Any(p => p.UserId == _userManager.GetUserIdAsync(user).Result),
                    IsUserApprovedParticipant = ev.Participants.Any(p => p.UserId == _userManager.GetUserIdAsync(user).Result && p.ParticipantStatusId == approvedStatus),
                    IsUserUndeterminedParticipant = ev.Participants.Any(p => p.UserId == _userManager.GetUserIdAsync(user).Result && p.ParticipantStatusId == undeterminedStatus),
                    IsUserRejectedParticipant = ev.Participants.Any(p => p.UserId == _userManager.GetUserIdAsync(user).Result && p.ParticipantStatusId == rejectedStatus),
                    IsEventApproved = ev.EventStatusID == approvedEvent,
                    IsEventNotApproved = ev.EventStatusID == notApprovedEvent,
                    IsEventFinished = ev.EventStatusID == finishedEvent,
                    EventAdmins = eventAdmins.Where(p => p.EventID == ev.ID).ToList()
                })
                .ToList();
        }
    }
}
