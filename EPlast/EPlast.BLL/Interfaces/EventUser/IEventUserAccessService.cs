using EPlast.DataAccess.Entities;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.EventUser
{
    public interface IEventUserAccessService
    {
        Task<bool> HasAccessAsync(User user, int eventId);
    }
}
