using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using EPlast.Models;

namespace EPlast.ViewModels
{
    public class DecisionViewModel
    {
        public DecisionWrapper DecisionWrapper { get; set; }
        public IEnumerable<SelectListItem> OrganizationListItems { get; set; }
        public IEnumerable<SelectListItem> DecisionStatusTypeListItems { get; set; }
        public IEnumerable<DecisionTarget> DecisionTargets { get; set; }
    }
}