using System;
using System.Collections.Generic;
using System.Text;
using EPlast.BLL.DTO.EventUser;
using EPlast.DataAccess.Entities.Event;

namespace EPlast.BLL.DTO.Events
{
    public class EventCategoryCreateDTO
    {
        public EventCategoryDTO EventCategory { get; set; }
        public int EventTypeId { get; set; }
    }
}
