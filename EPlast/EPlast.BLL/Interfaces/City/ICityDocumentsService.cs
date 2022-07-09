using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.BLL.DTO.City;

namespace EPlast.BLL.Interfaces.City
{
    public interface ICityDocumentsService
    {
        /// <summary>
        /// Add a document to the city
        /// </summary>
        /// <param name="document">An information about a specific document</param>
        /// <returns>A newly created document</returns>
        Task<CityDocumentsDto> AddDocumentAsync(CityDocumentsDto documentDTO);

        /// <summary>
        /// Get a file in base64 format
        /// </summary>
        /// <param name="logoName">The name of a city file</param>
        /// <returns>A base64 string of the file</returns>
        Task<string> DownloadFileAsync(string fileName);

        /// <summary>
        /// Get all city document types
        /// </summary>
        /// <returns>All city document types</returns>
        Task<IEnumerable<CityDocumentTypeDto>> GetAllCityDocumentTypesAsync();

        /// <summary>
        /// Delete a specific document by id
        /// </summary>
        /// <param name="documentId">The id of a specific document</param>
        Task DeleteFileAsync(int documentId);
    }
}
