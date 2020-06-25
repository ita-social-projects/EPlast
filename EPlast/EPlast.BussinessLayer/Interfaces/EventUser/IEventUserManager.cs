using EPlast.BussinessLayer.DTO.EventUser;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EPlast.BussinessLayer.Interfaces.EventUser
{
    public interface IEventUserManager
    {
        Task<EventUserDTO> EventUserAsync(string userId, ClaimsPrincipal user);
        Task<EventCreateDTO> InitializeEventCreateDTOAsync();
        Task<int> CreateEventAsync(EventCreateDTO model);
        Task<EventCreateDTO> InitializeEventEditDTOAsync(int eventId);
        Task EditEventAsync(EventCreateDTO model);
    }
}
