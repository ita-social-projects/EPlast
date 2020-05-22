using System.Collections.Generic;
using System.Security.Claims;
using EPlast.BussinessLayer.DTO.Events;
using Microsoft.AspNetCore.Http;

namespace EPlast.BussinessLayer.Interfaces.Events
{
    public interface IActionManager
    {
        List<EventCategoryDTO> GetActionCategories();
        List<GeneralEventDTO> GetEvents(int id, ClaimsPrincipal user);
        EventDTO GetEventInfo(int id, ClaimsPrincipal user);
        int DeleteEvent(int id);
        int SubscribeOnEvent(int id, ClaimsPrincipal user);
        int UnSubscribeOnEvent(int id, ClaimsPrincipal user);
        int ApproveParticipant(int id);
        int UnderReviewParticipant(int id);
        int RejectParticipant(int id);
        int FillEventGallery(int id, IList<IFormFile> files);
        int DeletePicture(int id);

    }
}
