using EPlast.BLL.DTO.Blank;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.Blank
{
    public interface IBlankAchievementDocumentService
    {
        /// <summary>
        /// Add a achievement document to the blank
        /// </summary>
        /// <param name="document">An information about a specific document</param>
        /// <returns>A newly created biography document</returns>
        Task<IEnumerable<AchievementDocumentsDTO>> AddDocumentAsync(IEnumerable<AchievementDocumentsDTO> achievementDocumentsDTO);

        /// <summary>
        /// Get a file in base64 format
        /// </summary>
        /// <param name="logoName">The name of a blank file</param>
        /// <returns>A base64 string of the file</returns>
        Task<string> DownloadFileAsync(string fileName);

        /// <summary>
        /// Delete a specific document by id
        /// </summary>
        /// <param name="documentId">The id of a specific document</param>
        Task DeleteFileAsync(int documentId);

        Task<IEnumerable<AchievementDocumentsDTO>> GetDocumentsByUserId(string userid);

        Task<IEnumerable<AchievementDocumentsDTO>> GetPartOfAchievement(int pageNumber, int pageSize, string userid);
    }
}
