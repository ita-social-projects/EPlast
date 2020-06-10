using EPlast.BussinessLayer.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BussinessLayer
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

        Task<List<OrganizationDTO>> GetOrganizationListAsync();

        Task<List<DecisionTargetDTO>> GetDecisionTargetListAsync();

        Task<List<DecisionWrapperDTO>> GetDecisionListAsync();

        IEnumerable<SelectListItem> GetDecisionStatusTypes();

        string GetContentType(int decisionId, string filename);
    }
}