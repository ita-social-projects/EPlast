using EPlast.BLL.DTO.Club;
using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.BLL.DTO.Events;
using EPlast.DataAccess.Entities;

namespace EPlast.BLL.Interfaces.EventUser
{
    public interface IEventUserAccessService
    {
        Task<bool> HasAccessAsync(User user, int eventId);
    }
}
