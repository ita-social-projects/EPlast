using EPlast.BLL.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Azure.Storage.Blob;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL
{
    public interface IDecisionService
    {
        Task<DecisionDTO> GetDecisionAsync(int decisionId);

        Task<bool> ChangeDecisionAsync(DecisionDTO decision);

        Task<bool> DeleteDecisionAsync(int id);

        Task<int> SaveDecisionAsync(DecisionWrapperDTO decision);


        DecisionWrapperDTO CreateDecision();

        Task<OrganizationDTO> GetDecisionOrganizationAsync(OrganizationDTO organization);

        Task<IEnumerable<OrganizationDTO>> GetOrganizationListAsync();

        Task<IEnumerable<DecisionTargetDTO>> GetDecisionTargetListAsync();

        Task<IEnumerable<DecisionWrapperDTO>> GetDecisionListAsync();

        IEnumerable<SelectListItem> GetDecisionStatusTypes();

         Task<string> DownloadDecisionFileFromBlobAsync(string fileName);
    }
}