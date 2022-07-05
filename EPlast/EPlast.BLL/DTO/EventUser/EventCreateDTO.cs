using System.Collections.Generic;
using EPlast.BLL.DTO.Events;
using Microsoft.AspNetCore.Mvc;

namespace EPlast.BLL.DTO.EventUser
{
    public class EventCreateDto
    {
        public EventCreationDto Event { get; set; }
        public EventAdministrationDto Сommandant { get; set; }
        public EventAdministrationDto Alternate { get; set; }
        public EventAdministrationDto Bunchuzhnyi { get; set; }
        public EventAdministrationDto Pysar { get; set; }
        public IEnumerable<EventCategoryDto> EventCategories { get; set; }
        public IEnumerable<EventTypeDto> EventTypes { get; set; }
        public IEnumerable<UserInfoDto> Users { get; set; }
    }
}
