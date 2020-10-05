using EPlast.BLL.DTO.Blank;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.Blank
{
   public interface IBlankBiographyDocumentService
    {
        /// <summary>
        /// Add a biography document to the blank
        /// </summary>
        /// <param name="document">An information about a specific document</param>
        /// <returns>A newly created biography document</returns>
        Task<BlankBiographyDocumentsDTO> AddDocumentAsync(BlankBiographyDocumentsDTO biographyDocumentDTO);

        /// <summary>
        /// Get a file in base64 format
        /// </summary>
        /// <param name="logoName">The name of a blank file</param>
        /// <returns>A base64 string of the file</returns>
        Task<string> DownloadFileAsync(string fileName);

        /// <summary>
        /// Get all biography blank document types
        /// </summary>
        /// <returns>All biography blank document types</returns>
        Task<IEnumerable<BlankBiographyDocumentsTypeDTO>> GetAllBlankDocumentTypesAsync();

        /// <summary>
        /// Delete a specific document by id
        /// </summary>
        /// <param name="documentId">The id of a specific document</param>
        Task DeleteFileAsync(int documentId);
    }
}
