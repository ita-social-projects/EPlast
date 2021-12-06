using EPlast.DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.EventUser
{
    public interface IEventUserAccessService
    {
        Task<bool> IsUserAdminOfEvent(User user, int eventId);

        Task<string> GetEventStatusAsync(User user, int eventId);

        Task<Dictionary<string, bool>> RedefineAccessesAsync(Dictionary<string, bool> userAccesses, User user,
            int? eventId = null);
    }
}
