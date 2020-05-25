using EPlast.BussinessLayer.DTO.Events;
using System.Collections.Generic;

namespace EPlast.BussinessLayer.Interfaces.Events
{
    public interface IEventCategoryManager
    {
        List<EventCategoryDTO> GetDTO();
    }
}
