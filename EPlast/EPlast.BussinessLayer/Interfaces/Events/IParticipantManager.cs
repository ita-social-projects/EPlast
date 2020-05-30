using System.Collections.Generic;
using EPlast.DataAccess.Entities;

namespace EPlast.BussinessLayer.Interfaces.Events
{
    public interface IParticipantManager
    {
        int SubscribeOnEvent(Event targetEvent, string userId);
        int UnSubscribeOnEvent(Event targetEvent, string userId);
        int ChangeStatusToApproved(int id);
        int ChangeStatusToUnderReview(int id);
        int ChangeStatusToRejected(int id);
        IEnumerable<Participant> GetParticipantsByUserId(string userId);
    }
}
