using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPlast.BLL.DTO.Events;

namespace EPlast.WebApi.Models.Events
{
    public class EventsCategoryViewModel
    {
        public EventsCategoryViewModel(int page, int pageSize, IEnumerable<EventCategoryDto> categories)
        {
            Events = categories.Skip((page - 1) * pageSize).Take(pageSize);
            Total = categories.Count();
        }

        public IEnumerable<EventCategoryDto> Events { get; set; }
        public int Total { get; set; }
    }
}
