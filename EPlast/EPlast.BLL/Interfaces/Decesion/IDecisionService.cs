using EPlast.BLL.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.DataAccess.Entities.Decision;

namespace EPlast.BLL
{   /// <summary>
    /// Implement  operations for work with decisions
    /// </summary>
    public interface IDecisionService
    {
        /// <summary>
        /// Returns decision dto object, if exist
        /// </summary>
        /// <param name="decisionId"> Decision id </param>
        /// <returns>decision dto</returns>
        Task<DecisionDTO> GetDecisionAsync(int decisionId);

        /// <summary>
        /// Returns all searched Decisions
        /// </summary>
        /// <param name="searchedData">Search string</param>
        /// <param name="page">Current page</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Searched Decisions</returns>
        IEnumerable<DecisionTableObject> GetDecisionsForTable(string searchedData, int page, int pageSize);

        /// <summary>
        /// Changes the name and description of the decision
        /// </summary>
        /// <param name="decisionDto">Decision dto</param>
        /// <returns>The bool value that says whether the changes were successful</returns>
        Task ChangeDecisionAsync(DecisionDTO decisionDto);

        /// <summary>
        /// Deletes the decision
        /// </summary>
        /// <param name="id">Decision id</param>
        /// <returns>The bool value that says whether the changes were successful</returns>
        Task DeleteDecisionAsync(int id);

        /// <summary>
        /// Adds new decision
        /// </summary>
        /// <param name="decision">The wrapper which contains decision dto and file as base64</param>
        /// <returns>An id of created decision</returns>
        Task<int> SaveDecisionAsync(DecisionWrapperDTO decision);

        /// <summary>
        /// Creates new decisions wrapper dto
        /// </summary>
        /// <returns> New decisions wrapper dto</returns>
        DecisionWrapperDTO CreateDecision();

        /// <summary>
        /// Returns the organizations dto
        /// </summary>
        /// <param name="governingBody"></param>
        /// <returns>The organizations dto and  null if organization does now exist</returns>
        Task<GoverningBodyDTO> GetDecisionOrganizationAsync(GoverningBodyDTO governingBody);

        /// <summary>
        /// Returns the IEnumerable of the organizations dto
        /// </summary>
        /// <returns>The IEnumerable of the organizations dto</returns>
        Task<IEnumerable<GoverningBodyDTO>> GetGoverningBodyListAsync();

        /// <summary>
        /// Returns the IEnumerable of the decision targets dto
        /// </summary>
        /// <returns>The IEnumerable of the decision targets dto</returns>
        Task<IEnumerable<DecisionTargetDTO>> GetDecisionTargetListAsync();

        /// <summary>
        /// Returns the IEnumerable of the decision targets dto 
        /// </summary>
        /// <returns>The IEnumerable of the decision targets dto</returns>
        Task<IEnumerable<DecisionTargetDTO>> GetDecisionTargetSearchListAsync(string search);

        /// <summary>
        /// Returns the IEnumerable of the decision wrappers dto
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<DecisionWrapperDTO>> GetDecisionListAsync();

        /// <summary>
        /// Returns the SelectListItem of the decisions status types
        /// </summary>
        /// <returns>The SelectListItem of the decisions status types</returns>
        IEnumerable<SelectListItem> GetDecisionStatusTypes();

        /// <summary>
        /// Returns file as base64
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <returns>File as base64</returns>
        Task<string> DownloadDecisionFileFromBlobAsync(string fileName);
    }
}
