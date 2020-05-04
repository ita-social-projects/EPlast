using EPlast.DataAccess.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace EPlast.ViewModels
{
    public class DecesionViewModel
    {
        public Decesion Decesion { get; set; }
        public IEnumerable<SelectListItem> OrganizationListItems { get; set; }
        public IEnumerable<DecesionTarget> DecesionTargets { get; set; }
        public IEnumerable<SelectListItem> DecesionStatusTypeListItems { get; set; }
        public IFormFile File { get; set; }
        public string Filename { get; set; }
    }
}