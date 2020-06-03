using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using EPlast.DataAccess.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPlast.DataAccess.Entities.Event;

namespace EPlast.Models.ViewModelInitializations.Interfaces
{
    public interface ICreateEventVMInitializer
    {
        IEnumerable<SelectListItem> GetEventCategories(IEnumerable<EventCategory> eventCategories);
    }
}