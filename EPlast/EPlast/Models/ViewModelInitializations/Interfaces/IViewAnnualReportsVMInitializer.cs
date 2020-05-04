using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using EPlast.DataAccess.Entities;

namespace EPlast.Models.ViewModelInitializations.Interfaces
{
    public interface IViewAnnualReportsVMInitializer
    {
        IEnumerable<SelectListItem> GetCities(IEnumerable<City> cities);
    }
}