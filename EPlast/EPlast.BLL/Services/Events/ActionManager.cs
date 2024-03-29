﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.Events;
using EPlast.BLL.DTO.EventUser;
using EPlast.BLL.DTO.Notification;
using EPlast.BLL.Interfaces.Events;
using EPlast.BLL.Interfaces.EventUser;
using EPlast.BLL.Interfaces.Notifications;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.Event;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
        private readonly INotificationService _notificationService;

        public ActionManager(UserManager<User> userManager, IRepositoryWrapper repoWrapper, IMapper mapper,
            IParticipantStatusManager participantStatusManager, IParticipantManager participantManager,
            IEventWrapper eventWrapper, INotificationService notificationService)
        {
            _userManager = userManager;
            _repoWrapper = repoWrapper;
            _mapper = mapper;
            _participantStatusManager = participantStatusManager;
            _participantManager = participantManager;
            _eventWrapper = eventWrapper;
            _notificationService = notificationService;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<EventTypeDto>> GetEventTypesAsync()
        {
            return await _eventWrapper.EventTypeManager.GetEventTypesDTOAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<EventCategoryDto>> GetActionCategoriesAsync()
        {
            return await _eventWrapper.EventCategoryManager.GetDTOAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<EventSectionDto>> GetEventSectionsAsync()
        {
            return await _eventWrapper.EventSectionManager.GetEventSectionsDTOAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<EventCategoryDto>> GetCategoriesByTypeIdAsync(int eventTypeId)
        {
            return await _eventWrapper.EventCategoryManager.GetDTOByEventTypeIdAsync(eventTypeId);
        }

        /// <inheritdoc />
        public async Task<EventCategoryDto> GetCategoryByIdAsync(int categoryId)
        {
            var category = await _repoWrapper.EventCategory.GetFirstOrDefaultAsync(c => c.ID == categoryId);
            if (category == null) return null;
            return _mapper.Map<EventCategory, EventCategoryDto>(category);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<EventCategoryDto>> GetCategoriesByPageAsync(int eventTypeId, int page, int pageSize)
        {
            return await _eventWrapper.EventCategoryManager.GetDTOByEventPageAsync(eventTypeId, page, pageSize);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<GeneralEventDto>> GetEventsAsync(int categoryId, int eventTypeId, User user)
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

        public async Task<Event> GetEventAsync(int eventId)
        {
            var eventEntity = await _repoWrapper.Event.GetFirstOrDefaultAsync(e => e.ID == eventId);
            return eventEntity;
        }

        /// <inheritdoc />
        public async Task<EventDto> GetEventInfoAsync(int id, User user)
        {
            int approvedStatus = await _participantStatusManager.GetStatusIdAsync("Учасник");
            int undeterminedStatus = await _participantStatusManager.GetStatusIdAsync("Розглядається");
            int rejectedStatus = await _participantStatusManager.GetStatusIdAsync("Відмовлено");
            int finishedEvent = await _eventWrapper.EventStatusManager.GetStatusIdAsync("Завершено");
            var userRoles = await _userManager.GetRolesAsync(user);
            bool isUserGlobalEventAdmin = userRoles?.Contains(Roles.EventAdministrator) ?? false;

            var targetEvent = await _repoWrapper.Event
                .GetFirstOrDefaultAsync(
                    e => e.ID == id,
                    source => source
                        .Include(e => e.Participants)
                        .ThenInclude(p => p.User)
                        .Include(e => e.Participants)
                        .ThenInclude(p => p.ParticipantStatus)
                        .Include(e => e.Participants)
                        .ThenInclude(p => p.EventFeedback)
                        .Include(e => e.EventStatus)
                        .Include(e => e.EventAdministrations)
                        .ThenInclude(a => a.User)
                        .Include(e => e.EventAdministrations)
                        .ThenInclude(a => a.EventAdministrationType)
                        .Include(e => e.EventType)
                        .Include(e => e.EventCategory)
                        .Include(e => e.EventGallarys)
                );

            if (targetEvent == null) return null;

            var eventInfo = _mapper.Map<Event, EventInfoDto>(targetEvent);
            eventInfo.Gallery = targetEvent.EventGallarys.Select(g => g.GallaryID).ToList();

            var userId = await _userManager.GetUserIdAsync(user);

            var dto = new EventDto()
            {
                Event = eventInfo,
                IsUserEventAdmin =
                    (targetEvent.EventAdministrations.Any(evAdm =>
                        evAdm.UserID == userId)) || isUserGlobalEventAdmin,
                IsUserParticipant =
                    targetEvent.Participants.Any(p => p.UserId == userId),
                IsUserApprovedParticipant = targetEvent.Participants.Any(p =>
                    p.UserId == userId && p.ParticipantStatusId == approvedStatus),
                IsUserUndeterminedParticipant = targetEvent.Participants.Any(p =>
                    p.UserId == userId &&
                    p.ParticipantStatusId == undeterminedStatus),
                IsUserRejectedParticipant = targetEvent.Participants.Any(p =>
                    p.UserId == userId && p.ParticipantStatusId == rejectedStatus),
                IsEventFinished = targetEvent.EventStatusID == finishedEvent
            };

            if (!dto.IsUserEventAdmin && dto.ParticipantAssessment != 0)
            {
                dto.Event.EventParticipants = dto.Event.EventParticipants.Where(p => p.StatusId == approvedStatus);
            }

            if (dto.IsUserApprovedParticipant
                && dto.IsEventFinished
                && (DateTime.Now < targetEvent.EventDateEnd.Add(TimeSpan.FromDays(3)))
                && !targetEvent.Participants.Any(p => p.UserId == userId && p.EventId == id && p.EventFeedback != null))
            {
                dto.CanEstimate = true;
                dto.ParticipantAssessment = targetEvent.Participants
                    .First(p => p.UserId == userId).Estimate;
            }

            return dto;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<EventGalleryDto>> GetPicturesAsync(int id)
        {
            return await _eventWrapper.EventGalleryManager.GetPicturesInBase64(id);
        }

        public async Task<EventGalleryDto> GetPictureAsync(int id)
        {
            return await _eventWrapper.EventGalleryManager.GetPictureByIdAsync(id);
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

        public async Task ChangeUsersPresentStatusAsync(int participantId)
        {
            await _participantManager.ChangeUserPresentStatusAsync(participantId);
        }

        public async Task LeaveFeedbackAsync(EventFeedbackDto feedback, Participant participant)
        {
            var existingFeedback = await _repoWrapper.EventFeedback.GetFirstOrDefaultAsync(f => f.ParticipantId == participant.ID);

            if (existingFeedback != null)
            {
                existingFeedback.Text = feedback.Text;
                existingFeedback.Rating = feedback.Rating;

                _repoWrapper.EventFeedback.Update(existingFeedback);
                await _repoWrapper.SaveAsync();
                return;
            }

            var createdFeedback = _mapper.Map<EventFeedbackDto, EventFeedback>(feedback);

            createdFeedback.ParticipantId = participant.ID;

            await _repoWrapper.EventFeedback.CreateAsync(createdFeedback);
            await _repoWrapper.SaveAsync();
        }

        public async Task DeleteFeedbackAsync(int feedbackId)
        {
            var eventFeedback = await _repoWrapper.EventFeedback.GetFirstOrDefaultAsync(f => f.Id == feedbackId);
            if (eventFeedback != null)
            {
                _repoWrapper.EventFeedback.Delete(eventFeedback);
                await _repoWrapper.SaveAsync();

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
        public async Task<IEnumerable<int>> FillEventGalleryAsync(int id, IList<IFormFile> files)
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
                .GetAllAsync(e => e.EventStatusID != finishedEventStatus && e.EventDateEnd < DateTime.Now, include: users =>
              users.Include(d => d.Participants));
            if (eventsToCheck.Any())
            {
                foreach (var eventToCheck in eventsToCheck)
                {
                    eventToCheck.EventStatusID = finishedEventStatus;
                    _repoWrapper.Event.Update(eventToCheck);
                    List<UserNotificationDto> userNotificationsDTO = new List<UserNotificationDto>();
                    foreach (var user in eventToCheck.Participants)
                    {
                        if ((!user.WasPresent && eventToCheck.EventDateEnd.Add(TimeSpan.FromDays(3)) < DateTime.Now) ||
                            user.ParticipantStatusId != 1) continue;

                        userNotificationsDTO.Add(new UserNotificationDto
                        {
                            Message = "Тепер ви можете залишити відгук про подію ",
                            NotificationTypeId = 1,
                            OwnerUserId = user.UserId,
                            SenderLink = $"/events/details/{eventToCheck.ID}",
                            SenderName = '\'' + eventToCheck.EventName + "'."
                        });
                    }
                    await _notificationService.AddListUserNotificationAsync(userNotificationsDTO);
                }
                await _repoWrapper.SaveAsync();
            }
        }

        public async Task<IEnumerable<GeneralEventDto>> GetEventsByStatusAsync(int categoryId, int typeId, int status, User user)
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

        private async Task<List<GeneralEventDto>> GetEventDtosAsync(IEnumerable<Event> events, User user)
        {
            int approvedStatus = await _participantStatusManager.GetStatusIdAsync("Учасник");
            int undeterminedStatus = await _participantStatusManager.GetStatusIdAsync("Розглядається");
            int rejectedStatus = await _participantStatusManager.GetStatusIdAsync("Відмовлено");
            int approvedEvent = await _eventWrapper.EventStatusManager.GetStatusIdAsync("Затверджено");
            int finishedEvent = await _eventWrapper.EventStatusManager.GetStatusIdAsync("Завершено");
            int notApprovedEvent = await _eventWrapper.EventStatusManager.GetStatusIdAsync("Не затверджено");
            var userRoles = await _userManager.GetRolesAsync(user);

            var eventAdmins = await _repoWrapper.EventAdministration.GetAllAsync();

            return events
                .Select(ev => new GeneralEventDto
                {
                    EventId = ev.ID,
                    EventName = ev.EventName,
                    IsUserEventAdmin = ev.EventAdministrations.Any(e => e.UserID == _userManager.GetUserIdAsync(user).Result) || userRoles != null && userRoles.Contains(Roles.EventAdministrator),
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
