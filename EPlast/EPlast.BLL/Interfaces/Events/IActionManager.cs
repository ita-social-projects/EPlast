using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using EPlast.BLL.DTO.Events;
using Microsoft.AspNetCore.Http;

namespace EPlast.BLL.Interfaces.Events
{
    public interface IActionManager
    {
        Task<List<EventCategoryDTO>> GetActionCategoriesAsync();
        Task<List<GeneralEventDTO>> GetEventsAsync(int id, ClaimsPrincipal user);
        Task<EventDTO> GetEventInfoAsync(int id, ClaimsPrincipal user);
        Task<int> DeleteEventAsync(int id);
        Task<int> SubscribeOnEventAsync(int id, ClaimsPrincipal user);
        Task<int> UnSubscribeOnEventAsync(int id, ClaimsPrincipal user);
        Task<int> ApproveParticipantAsync(int id);
        Task<int> UnderReviewParticipantAsync(int id);
        Task<int> RejectParticipantAsync(int id);
        Task<int> FillEventGalleryAsync(int id, IList<IFormFile> files);
        Task<int> DeletePictureAsync(int id);

    }
}
