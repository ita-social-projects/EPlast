using EPlast.BLL.DTO.GoverningBody;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.GoverningBodies
{
    public interface IGoverningBodyDocumentsService
    {
        /// <summary>
        /// Add a document to the Governing Body
        /// </summary>
        /// <param name="documentDto">An information about a specific document</param>
        /// <returns>A newly created document</returns>
        Task<GoverningBodyDocumentsDTO> AddGoverningBodyDocumentAsync(GoverningBodyDocumentsDTO documentDto);

        /// <summary>
        /// Get a file in base64 format
        /// </summary>
        /// <param name="documentName">The name of a Governing Body document</param>
        /// <returns>A base64 string of the document</returns>
        Task<string> DownloadGoverningBodyDocumentAsync(string documentName);

        /// <summary>
        /// Get all Governing Body document types
        /// </summary>
        /// <returns>All Governing Body document types</returns>
        Task<IEnumerable<GoverningBodyDocumentTypeDTO>> GetAllGoverningBodyDocumentTypesAsync();

        /// <summary>
        /// Delete a specific document by id
        /// </summary>
        /// <param name="documentId">The id of a specific document</param>
        Task DeleteGoverningBodyDocumentAsync(int documentId);
    }
}
