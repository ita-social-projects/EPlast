using System.Collections.Generic;
using EPlast.BussinessLayer.Interfaces.Events;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Http;
using System.Linq;
using EPlast.DataAccess.Entities.Event;
using Microsoft.EntityFrameworkCore;
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

        public int SubscribeOnEvent(Event targetEvent, string userId)
        {
            try
            {
                int undeterminedStatus = _participantStatusManager.GetStatusId("Розглядається");
                int finishedEvent = _eventStatusManager.GetStatusId("Завершений(-на)");
                if (targetEvent.EventStatusID == finishedEvent)
                {
                    return StatusCodes.Status409Conflict;
                }
                _repoWrapper.Participant.Create(new Participant() { ParticipantStatusId = undeterminedStatus, EventId = targetEvent.ID, UserId = userId });
                _repoWrapper.Save();
                return StatusCodes.Status200OK;
            }
            catch
            {
                return StatusCodes.Status500InternalServerError;
            }
        }

        public int UnSubscribeOnEvent(Event targetEvent, string userId)
        {
            try
            {
                int rejectedStatus = _participantStatusManager.GetStatusId("Відмовлено");
                int finishedEvent = _eventStatusManager.GetStatusId("Завершений(-на)");
                Participant participantToDelete = _repoWrapper.Participant.FindByCondition(p => p.EventId == targetEvent.ID && p.UserId == userId).First();
                if (participantToDelete.ParticipantStatusId == rejectedStatus || targetEvent.EventStatusID == finishedEvent)
                {
                    return StatusCodes.Status409Conflict;
                }
                _repoWrapper.Participant.Delete(participantToDelete);
                _repoWrapper.Save();
                return StatusCodes.Status200OK;
            }
            catch
            {
                return StatusCodes.Status500InternalServerError;
            }
        }

        public int ChangeStatusToApproved(int id)
        {
            try
            {
                Participant participant = _repoWrapper.Participant.FindByCondition(p => p.ID == id).First();
                int approvedStatus = _participantStatusManager.GetStatusId("Учасник");
                participant.ParticipantStatusId = approvedStatus;
                _repoWrapper.Participant.Update(participant);
                _repoWrapper.Save();
                return StatusCodes.Status200OK;
            }
            catch
            {
                return StatusCodes.Status500InternalServerError;
            }
        }

        public int ChangeStatusToUnderReview(int id)
        {
            try
            {
                Participant participant = _repoWrapper.Participant.FindByCondition(p => p.ID == id).First();
                int undeterminedStatus = _participantStatusManager.GetStatusId("Розглядається");
                participant.ParticipantStatusId = undeterminedStatus;
                _repoWrapper.Participant.Update(participant);
                _repoWrapper.Save();
                return StatusCodes.Status200OK;
            }
            catch
            {
                return StatusCodes.Status500InternalServerError;
            }
        }

        public int ChangeStatusToRejected(int id)
        {
            try
            {
                Participant participant = _repoWrapper.Participant.FindByCondition(p => p.ID == id).First();
                int rejectedStatus = _participantStatusManager.GetStatusId("Відмовлено");
                participant.ParticipantStatusId = rejectedStatus;
                _repoWrapper.Participant.Update(participant);
                _repoWrapper.Save();
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
                .Include(i => i.Event).ToListAsync();
            return participants;
        }
    }
}
