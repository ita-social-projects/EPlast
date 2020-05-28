using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using EPlast.BussinessLayer.DTO.EventUser;

namespace EPlast.BussinessLayer.Interfaces.EventUser
{
    public interface IEventUserManager
    {
        EventUserDTO EventUser(string userId, ClaimsPrincipal user);
        EventCreateDTO InitializeEventCreateDTO();
        EventCreateDTO InitializeEventCreateDTO(int eventId);
        int CreateEvent(EventCreateDTO model);
        void SetAdministration(EventCreateDTO model);
    }
}
