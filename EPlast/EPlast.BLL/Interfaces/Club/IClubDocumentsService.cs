using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.BLL.DTO.Club;

namespace EPlast.BLL.Interfaces.Club
{
    public interface IClubDocumentsService
    {

        /// <summary>
        /// Add a document to the Club
        /// </summary>
        /// <param name="document">An information about a specific document</param>
        /// <returns>A newly created document</returns>
        Task<ClubDocumentsDto> AddDocumentAsync(ClubDocumentsDto documentsDTO);

        /// <summary>
        /// Get a file in base64 format
        /// </summary>
        /// <param name="logoName">The name of a Club file</param>
        /// <returns>A base64 string of the file</returns>
        Task<string> DownloadFileAsync(string fileName);

        /// <summary>
        /// Get all Club document types
        /// </summary>
        /// <returns>All Club document types</returns>
        Task<IEnumerable<ClubDocumentTypeDto>> GetAllClubDocumentTypesAsync();

        /// <summary>
        /// Delete a specific document by id
        /// </summary>
        /// <param name="documentId">The id of a specific document</param>
        Task DeleteFileAsync(int documentId);
    }
}
