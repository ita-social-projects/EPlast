using System.Collections.Generic;
using System.Security.Claims;
using EPlast.BussinessLayer.DTO.Events;
using EPlast.BussinessLayer.Services;
using Microsoft.AspNetCore.Http;

namespace EPlast.BussinessLayer.Interfaces
{
   public interface IActionManager
    {
        List<EventCategoryDTO> GetActionCategories();
        List<GeneralEventDTO> GetEvents(int id, ClaimsPrincipal user);
        EventDTO GetEventInfo(int id, ClaimsPrincipal user);
        Status DeleteEvent(int id);
        Status SubscribeOnEvent(int id, ClaimsPrincipal user);
        Status UnSubscribeOnEvent(int id, ClaimsPrincipal user);
        Status ApproveParticipant(int id);
        Status UnderReviewParticipant(int id);
        Status RejectParticipant(int id);
        Status FillEventGallery(int id, IList<IFormFile> files);
        Status DeletePicture(int id);

    }
}
