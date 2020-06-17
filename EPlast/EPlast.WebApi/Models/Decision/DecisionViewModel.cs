using EPlast.BussinessLayer.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace EPlast.WebApi.Models.Decision
{
    public class DecisionViewModel
    {
        public DecisionWrapperDTO DecisionWrapper { get; set; }
        public IEnumerable<SelectListItem> OrganizationListItems { get; set; }
        public IEnumerable<SelectListItem> DecisionStatusTypeListItems { get; set; }
        public IEnumerable<DecisionTargetDTO> DecisionTargets { get; set; }
    }
}
