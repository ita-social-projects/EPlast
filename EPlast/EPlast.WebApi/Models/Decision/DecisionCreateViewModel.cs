using System.Collections.Generic;
using EPlast.BLL.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EPlast.WebApi.Models.Decision
{
    public class DecisionCreateViewModel
    {
        public IEnumerable<GoverningBodyDto> GoverningBodies { get; set; }
        public IEnumerable<SelectListItem> DecisionStatusTypeListItems { get; set; }
        public IEnumerable<DecisionTargetDto> DecisionTargets { get; set; }

    }
}
