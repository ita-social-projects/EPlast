using EPlast.BussinessLayer.DTO.EventUser;
using System.Security.Claims;

namespace EPlast.BussinessLayer.Interfaces.EventUser
{
    public interface IEventUserManager
    {
        EventUserDTO EventUser(string userId, ClaimsPrincipal user);
        EventCreateDTO InitializeEventCreateDTO();
        EventCreateDTO InitializeEventCreateDTO(int eventId);
        int CreateEvent(EventCreateDTO model);
        void SetAdministration(EventCreateDTO model);
        EventCreateDTO InitializeEventEditDTO(int eventId);
        void EditEvent(EventCreateDTO model);
    }
}
