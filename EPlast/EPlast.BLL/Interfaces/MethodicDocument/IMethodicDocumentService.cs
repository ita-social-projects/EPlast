using EPlast.BLL.DTO;
using EPlast.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL
{
    public interface IMethodicDocumentService
    {
        /// <summary>
        /// Returns MethodicDocument dto object, if exist 
        /// </summary>
        /// <param name="documentId"> MethodicDocument id </param>
        /// <returns>MethodicDocument dto</returns>
        Task<MethodicDocumentDTO> GetMethodicDocumentAsync(int documentId);
        /// <summary>
        /// Changes the name and description of the MethodicDocument
        /// </summary>
        /// <param name="decisionDto">MethodicDocument dto</param>
        /// <returns>The bool value that says whether the changes were successful</returns>
        Task ChangeMethodicDocumentAsync(MethodicDocumentDTO documentDto);
        /// <summary>
        /// Deletes the MethodicDocument
        /// </summary>
        /// <param name="id">MethodicDocument id</param>
        /// <returns>The bool value that says whether the changes were successful</returns>
        Task DeleteMethodicDocumentAsync(int id);
        /// <summary>
        /// Adds new MethodicDocument
        /// </summary>
        /// <param name="decision">The wrapper which contains MethodicDocument dto and file as base64</param>
        /// <returns>An id of created MethodicDocument</returns>
        Task<int> SaveMethodicDocumentAsync(MethodicDocumentWraperDTO document);

        /// <summary>
        /// Creates new MethodicDocument wrapper dto
        /// </summary>
        /// <returns> New MethodicDocument wrapper dto</returns>
        MethodicDocumentWraperDTO CreateMethodicDocument();
        /// <summary>
        /// Returns the organizations dto 
        /// </summary>
        /// <param name="governingBody"></param>
        /// <returns>The organizations dto and  null if organization does now exist</returns>
        Task<GoverningBodyDTO> GetMethodicDocumentOrganizationAsync(GoverningBodyDTO governingBody);
        /// <summary>
        /// Returns the IEnumerable of the organizations dto
        /// </summary>
        /// <returns>The IEnumerable of the organizations dto</returns>
        Task<IEnumerable<GoverningBodyDTO>> GetGoverningBodyListAsync();

        /// <summary>
        /// Returns the IEnumerable of the MethodicDocument wrappers dto
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<MethodicDocumentWraperDTO>> GetMethodicDocumentListAsync();
        /// <summary>
        /// Returns the SelectListItem of the MethodicDocument types
        /// </summary>
        /// <returns>The SelectListItem of the MethodicDocument  types</returns>
        IEnumerable<SelectListItem> GetMethodicDocumentTypes();
        /// <summary>
        /// Returns file as base64
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <returns>File as base64</returns>
        Task<string> DownloadMethodicDocumentFileFromBlobAsync(string fileName);
        IEnumerable<MethodicDocumentTableObject> GetDocumentsForTable(string searchedData, int page, int pageSize, string status);
    }
}
