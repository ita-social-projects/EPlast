using EPlast.BLL;
using EPlast.BLL.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.WebApi.Models.Decision
{
    public class DecisionViewModel
    {
        public DecisionWrapperDTO DecisionWrapper { get; set; }
        public IEnumerable<SelectListItem> OrganizationListItems { get; set; }
        public IEnumerable<SelectListItem> DecisionStatusTypeListItems { get; set; }
        public IEnumerable<DecisionTargetDTO> DecisionTargets { get; set; }
        public static async Task<DecisionViewModel> GetNewDecisionViewModel(IDecisionService decisionService)
        {
            var organizations = await decisionService.GetOrganizationListAsync();

            DecisionViewModel decisionViewModel = new DecisionViewModel
            {
                DecisionWrapper = await decisionService.CreateDecisionAsync(),
                OrganizationListItems = from item in organizations
                                        select new SelectListItem
                                        {
                                            Text = item.OrganizationName,
                                            Value = item.ID.ToString()
                                        },
                DecisionTargets = await decisionService.GetDecisionTargetListAsync(),
                DecisionStatusTypeListItems = decisionService.GetDecisionStatusTypes()
            };

            return decisionViewModel;
        }

    }
}
