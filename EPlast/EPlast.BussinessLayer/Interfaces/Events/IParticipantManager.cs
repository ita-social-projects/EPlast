using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.Event;

namespace EPlast.BussinessLayer.Interfaces.Events
{
    public interface IParticipantManager
    {
        Task<int> SubscribeOnEventAsync(Event targetEvent, string userId);
        Task<int> UnSubscribeOnEventAsync(Event targetEvent, string userId);
        Task<int> ChangeStatusToApprovedAsync(int id);
        Task<int> ChangeStatusToUnderReviewAsync(int id);
        Task<int> ChangeStatusToRejectedAsync(int id);
        Task<IEnumerable<Participant>> GetParticipantsByUserIdAsync(string userId);
    }
}
