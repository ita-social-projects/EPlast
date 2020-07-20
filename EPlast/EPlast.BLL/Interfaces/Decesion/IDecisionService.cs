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

        Task<byte[]> DownloadDecisionFileAsync(int decisionId);

        Task<DecisionWrapperDTO> CreateDecisionAsync();

        Task<OrganizationDTO> GetDecisionOrganizationAsync(OrganizationDTO organization);

        Task<IEnumerable<OrganizationDTO>> GetOrganizationListAsync();

        Task<IEnumerable<DecisionTargetDTO>> GetDecisionTargetListAsync();

        Task<IEnumerable<DecisionWrapperDTO>> GetDecisionListAsync();

        IEnumerable<SelectListItem> GetDecisionStatusTypes();

        string GetContentType(int decisionId, string filename);
         Task<string> DownloadDecisionFileFromBlobAsync(string fileName);
    }
}