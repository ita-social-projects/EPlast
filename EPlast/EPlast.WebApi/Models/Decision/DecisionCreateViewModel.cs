using EPlast.BLL.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace EPlast.WebApi.Models.Decision
{
    public class DecisionCreateViewModel
    {
        public IEnumerable<GoverningBodyDTO> GoverningBodies { get; set; }
        public IEnumerable<SelectListItem> DecisionStatusTypeListItems { get; set; }
        public IEnumerable<DecisionTargetDTO> DecisionTargets { get; set; }

    }
}
