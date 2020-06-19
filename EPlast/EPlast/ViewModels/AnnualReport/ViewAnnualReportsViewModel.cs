using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using CityVMs = EPlast.ViewModels.City;

namespace EPlast.ViewModels.AnnualReport
{
    public class ViewAnnualReportsViewModel
    {
        public IEnumerable<AnnualReportViewModel> AnnualReports { get; set; }
        public IEnumerable<SelectListItem> Cities { get; set; }

        public ViewAnnualReportsViewModel(IEnumerable<CityVMs.CityViewModel> cities)
        {
            this.InitializeCities(cities);
        }

        private void InitializeCities(IEnumerable<CityVMs.CityViewModel> cities)
        {
            Cities = new List<SelectListItem>();
            foreach (var city in cities)
            {
                Cities = Cities.Append(new SelectListItem
                {
                    Value = city.ID.ToString(),
                    Text = city.Name
                });
            }
        }
    }
}