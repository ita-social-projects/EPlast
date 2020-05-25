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

        bool DeleteDecision(int id);

        Task<bool> SaveDecisionAsync(DecisionWrapperDTO decision);

        Task<byte[]> DownloadDecisionFileAsync(int decisionId);

        DecisionWrapperDTO CreateDecision();

        OrganizationDTO GetDecisionOrganization(int decisionId);

        List<OrganizationDTO> GetOrganizationList();

        List<DecisionTargetDTO> GetDecisionTargetList();

        List<DecisionWrapperDTO> GetDecisionList();

        IEnumerable<SelectListItem> GetDecisionStatusTypes();

        string GetContentType(int decisionId, string filename);
    }
}