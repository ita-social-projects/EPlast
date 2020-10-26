using EPlast.BLL.DTO.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.WebApi.Models.Events
{
    public class EventsCategoryViewModel
    {
        public EventsCategoryViewModel(int page, int pageSize, IEnumerable<EventCategoryDTO> categories)
        {
            Events = categories.Skip((page - 1) * pageSize).Take(pageSize);
            Total = categories.Count();
        }

        public IEnumerable<EventCategoryDTO> Events { get; set; }
        public int Total { get; set; }
    }
}
