using EPlast.ViewModels.City;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace EPlast.ViewModels
{
    public class ViewAnnualReportsViewModel
    {
        public IEnumerable<AnnualReportViewModel> AnnualReports { get; set; }
        public IEnumerable<SelectListItem> Cities { get; set; }

        public ViewAnnualReportsViewModel(IEnumerable<CityViewModel> cities)
        {
            this.InitializeCities(cities);
        }

        private void InitializeCities(IEnumerable<CityViewModel> cities)
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