using System.Collections.Generic;
using EPlast.BussinessLayer.Interfaces.Events;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;
using EPlast.DataAccess.Entities.Event;
using Microsoft.EntityFrameworkCore;

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
                Participant participantToDelete = await _repoWrapper.Participant.FindByCondition(p => p.EventId == targetEvent.ID && p.UserId == userId).FirstAsync();
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
                Participant participant = await _repoWrapper.Participant.FindByCondition(p => p.ID == id).FirstAsync();
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
                Participant participant = await _repoWrapper.Participant.FindByCondition(p => p.ID == id).FirstAsync();
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
                Participant participant = await _repoWrapper.Participant.FindByCondition(p => p.ID == id).FirstAsync();
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
            var participants = await _repoWrapper.Participant.FindByCondition(p => p.UserId == userId)
                .Include(i => i.Event)
                .ToListAsync();
            return participants;
        }
    }
}
