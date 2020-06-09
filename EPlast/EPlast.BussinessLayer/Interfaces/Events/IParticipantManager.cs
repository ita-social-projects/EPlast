using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.Event;

namespace EPlast.BussinessLayer.Interfaces.Events
{
    public interface IParticipantManager
    {
        int SubscribeOnEvent(Event targetEvent, string userId);
        int UnSubscribeOnEvent(Event targetEvent, string userId);
        int ChangeStatusToApproved(int id);
        int ChangeStatusToUnderReview(int id);
        int ChangeStatusToRejected(int id);
        Task<IEnumerable<Participant>> GetParticipantsByUserIdAsync(string userId);
    }
}
