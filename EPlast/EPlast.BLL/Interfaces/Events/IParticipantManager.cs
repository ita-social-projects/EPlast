﻿using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.Event;

namespace EPlast.BLL.Interfaces.Events
{
    /// <summary>
    ///  Implements  operations for work with event participants.
    /// </summary>
    public interface IParticipantManager
    {
        /// <summary>
        /// Create new event participant.
        /// </summary>
        /// <returns>Status code of the subscribing on event operation.</returns>
        /// <param name="targetEvent">The instance of event</param>
        /// <param name="userId">The Id of logged in user</param>
        Task<int> SubscribeOnEventAsync(Event targetEvent, string userId);

        /// <summary>
        /// Delete event participant.
        /// </summary>
        /// <returns>Status code of the unsubscribing on event operation.</returns>
        /// <param name="targetEvent">The instance of event</param>
        /// <param name="userId">The Id of logged in user</param>
        Task<int> UnSubscribeOnEventAsync(Event targetEvent, string userId);

        /// <summary>
        /// Change event participant status to approved.
        /// </summary>
        /// <returns>Status code of the changing event participant status operation.</returns>
        /// <param name="id">The Id of event participant</param>
        Task<int> ChangeStatusToApprovedAsync(int id);

        /// <summary>
        /// Change event participant status to under reviewed.
        /// </summary>
        /// <returns>Status code of the changing event participant status operation.</returns>
        /// <param name="id">The Id of event participant</param>
        Task<int> ChangeStatusToUnderReviewAsync(int id);

        /// <summary>
        /// Change event participant status to rejected.
        /// </summary>
        /// <returns>Status code of the changing event participant status operation.</returns>
        /// <param name="id">The Id of event participant</param>
        Task<int> ChangeStatusToRejectedAsync(int id);

        /// <summary>
        /// Get list of event participants by userId.
        /// </summary>
        /// <returns>List of event participants.</returns>
        /// <param name="userId">The Id of logged in user</param>
        Task<IEnumerable<Participant>> GetParticipantsByUserIdAsync(string userId);

        Task<Participant> GetParticipantByEventIdAndUserIdAsync(int eventId, string userId);

        Task<EventFeedback> GetEventFeedbackByIdAsync(int feedbackId);

        /// <summary>
        /// Change present status of the participant's event.
        /// </summary>
        /// <param name="perticipantId">The Id of participant</param>
        Task ChangeUserPresentStatusAsync(int perticipantId);
    }
}
