using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using EPlast.DataAccess.Entities;
using EPlast.Models.ViewModelInitializations.Interfaces;

namespace EPlast.Models.ViewModelInitializations
{
    public class ViewAnnualReportsVMInitializer : IViewAnnualReportsVMInitializer
    {
        public IEnumerable<SelectListItem> GetCities(IEnumerable<City> cities)
        {
            var citiesSLI = new List<SelectListItem>();
            foreach (var city in cities)
            {
                citiesSLI.Add(new SelectListItem
                {
                    Value = city.ID.ToString(),
                    Text = $"{city.Name}"
                });
            }
            return citiesSLI;
        }
    }
}