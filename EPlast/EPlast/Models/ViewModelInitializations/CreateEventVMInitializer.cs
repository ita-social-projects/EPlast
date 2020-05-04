using EPlast.DataAccess.Entities;
using EPlast.Models.ViewModelInitializations.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.Models.ViewModelInitializations
{
    public class CreateEventVMInitializer : ICreateEventVMInitializer
    {
        public IEnumerable<SelectListItem> GetEventCategories(IEnumerable<EventCategory> eventCategories)
        {
            var categories = new List<SelectListItem>
            {
                new SelectListItem { Text = "" }
            };
            foreach (var eventCategory in eventCategories)
            {
                categories.Add(new SelectListItem
                {
                    Value = eventCategory.ID.ToString(),
                    Text = $"{eventCategory.EventCategoryName}"
                });
            }
            return categories;
        }
    }
}