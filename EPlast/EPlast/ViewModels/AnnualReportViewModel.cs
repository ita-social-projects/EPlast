using System.Collections.Generic;
using EPlast.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EPlast.ViewModels
{
    public enum AnnualReportOperation
    {
        Creating,
        Editing
    }

    public class AnnualReportViewModel
    {
        public AnnualReportOperation Operation { get; set; }
        public string CityName { get; set; }
        public IEnumerable<SelectListItem> CityMembers { get; set; }
        public IEnumerable<SelectListItem> CityLegalStatusTypes { get; set; }
        public AnnualReport AnnualReport { get; set; }
    }
}