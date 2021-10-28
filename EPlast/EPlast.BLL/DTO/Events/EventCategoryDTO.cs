using System.Collections.Generic;
using EPlast.DataAccess.Entities.Event;

namespace EPlast.BLL.DTO.Events
{
    public class EventCategoryDTO
    {
        public int EventCategoryId { get; set; }
        public string EventCategoryName { get; set; }
        public int EventSectionId { get; set; }
    }
}
