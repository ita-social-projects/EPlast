using EPlast.BLL.DTO.Events;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.Event;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.EventUser
{
    public interface IEventUserAccessService
    {
        Task<bool> IsUserAdminOfEvent(User user, int eventId);

        Task<bool> CanPostFeedback(Participant user, int eventId);

        Task<bool> CanDeleteFeedback(User user, EventFeedback feedback);

        Task<Dictionary<string, bool>> RedefineAccessesAsync(Dictionary<string, bool> userAccesses, User user,
            int? eventId = null);
    }
}
