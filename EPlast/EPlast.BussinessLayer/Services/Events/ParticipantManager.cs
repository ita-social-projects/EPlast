using EPlast.BussinessLayer.Interfaces.Events;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.Event;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BussinessLayer.Services.Events
{
    public class ParticipantManager : IParticipantManager
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IEventStatusManager _eventStatusManager;
        private readonly IParticipantStatusManager _participantStatusManager;

        public ParticipantManager(IRepositoryWrapper repoWrapper, IEventStatusManager eventStatusManager, IParticipantStatusManager participantStatusManager)
        {
            _repoWrapper = repoWrapper;
            _eventStatusManager = eventStatusManager;
            _participantStatusManager = participantStatusManager;
        }

        public async Task<int> SubscribeOnEventAsync(Event targetEvent, string userId)
        {
            try
            {
                int undeterminedStatus = await _participantStatusManager.GetStatusIdAsync("Розглядається");
                int finishedEvent = await _eventStatusManager.GetStatusIdAsync("Завершений(-на)");
                if (targetEvent.EventStatusID == finishedEvent)
                {
                    return StatusCodes.Status409Conflict;
                }
                await _repoWrapper.Participant.CreateAsync(new Participant() { ParticipantStatusId = undeterminedStatus, EventId = targetEvent.ID, UserId = userId });
                await _repoWrapper.SaveAsync();
                return StatusCodes.Status200OK;
            }
            catch
            {
                return StatusCodes.Status500InternalServerError;
            }
        }

        public async Task<int> UnSubscribeOnEventAsync(Event targetEvent, string userId)
        {
            try
            {
                int rejectedStatus = await _participantStatusManager.GetStatusIdAsync("Відмовлено");
                int finishedEvent = await _eventStatusManager.GetStatusIdAsync("Завершений(-на)");
                Participant participantToDelete = await _repoWrapper.Participant
                    .GetFirstAsync(predicate: p => p.EventId == targetEvent.ID && p.UserId == userId);
                if (participantToDelete.ParticipantStatusId == rejectedStatus || targetEvent.EventStatusID == finishedEvent)
                {
                    return StatusCodes.Status409Conflict;
                }
                _repoWrapper.Participant.Delete(participantToDelete);
                await _repoWrapper.SaveAsync();

                return StatusCodes.Status200OK;
            }
            catch
            {
                return StatusCodes.Status500InternalServerError;
            }
        }

        public async Task<int> ChangeStatusToApprovedAsync(int id)
        {
            try
            {
                var participant = await _repoWrapper.Participant
                    .GetFirstAsync(predicate: p => p.ID == id);
                int approvedStatus = await _participantStatusManager.GetStatusIdAsync("Учасник");
                participant.ParticipantStatusId = approvedStatus;
                _repoWrapper.Participant.Update(participant);
                await _repoWrapper.SaveAsync();

                return StatusCodes.Status200OK;
            }
            catch
            {
                return StatusCodes.Status500InternalServerError;
            }
        }

        public async Task<int> ChangeStatusToUnderReviewAsync(int id)
        {
            try
            {
                var participant = await _repoWrapper.Participant
                    .GetFirstAsync(predicate: p => p.ID == id);
                int undeterminedStatus = await _participantStatusManager.GetStatusIdAsync("Розглядається");
                participant.ParticipantStatusId = undeterminedStatus;
                _repoWrapper.Participant.Update(participant);
                await _repoWrapper.SaveAsync();

                return StatusCodes.Status200OK;
            }
            catch
            {
                return StatusCodes.Status500InternalServerError;
            }
        }

        public async Task<int> ChangeStatusToRejectedAsync(int id)
        {
            try
            {
                var participant = await _repoWrapper.Participant
                    .GetFirstAsync(predicate: p => p.ID == id);
                int rejectedStatus = await _participantStatusManager.GetStatusIdAsync("Відмовлено");
                participant.ParticipantStatusId = rejectedStatus;
                _repoWrapper.Participant.Update(participant);
                await _repoWrapper.SaveAsync();

                return StatusCodes.Status200OK;
            }
            catch
            {
                return StatusCodes.Status500InternalServerError;
            }
        }

        public async Task<IEnumerable<Participant>> GetParticipantsByUserIdAsync(string userId)
        {

            var participants = await _repoWrapper.Participant
                .GetAllAsync(
                    predicate: p => p.UserId == userId,
                    include: source => source.Include(i => i.Event)
                );

            return participants;
        }
    }
}
