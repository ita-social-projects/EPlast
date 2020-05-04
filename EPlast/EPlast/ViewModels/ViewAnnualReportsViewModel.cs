using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using EPlast.DataAccess.Entities;

namespace EPlast.ViewModels
{
    public class ViewAnnualReportsViewModel
    {
        public IEnumerable<AnnualReport> AnnualReports { get; set; }
        public IEnumerable<SelectListItem> Cities { get; set; }
    }
}