using EPlast.BussinessLayer.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BussinessLayer
{
    public interface IDecisionService
    {
        DecisionDTO GetDecision(int decisionId);

        bool ChangeDecision(DecisionDTO decision);

        Task<bool> SaveDecision(DecisionWrapperDTO decision);

        Task<byte[]> DownloadDecisionFile(int decisionId);

        DecisionWrapperDTO CreateDecision();

        OrganizationDTO GetDecisionOrganization(int decisionId);

        List<OrganizationDTO> GetOrganizationList();

        List<DecisionTargetDTO> GetDecisionTargetList();

        List<DecisionWrapperDTO> GetDecisionList();

        IEnumerable<SelectListItem> GetDecisionStatusTypes();

        string GetContentType(int decisionId, string filename);
    }
}