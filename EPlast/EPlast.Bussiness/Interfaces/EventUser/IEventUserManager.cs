using EPlast.Bussiness.DTO.EventUser;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EPlast.Bussiness.Interfaces.EventUser
{
    public interface IEventUserManager
    {
        Task<EventUserDTO> EventUserAsync(string userId, ClaimsPrincipal user);
        Task<EventCreateDTO> InitializeEventCreateDTOAsync();
        Task<EventCreateDTO> InitializeEventCreateDTOAsync(int eventId);
        Task<int> CreateEventAsync(EventCreateDTO model);
        Task SetAdministrationAsync(EventCreateDTO model);
        Task<EventCreateDTO> InitializeEventEditDTOAsync(int eventId);
        Task EditEventAsync(EventCreateDTO model);
    }
}
