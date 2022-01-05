using EPlast.BLL.DTO.GoverningBody.Announcement;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.GoverningBodies
{
    public interface IGoverningBodyAnnouncementService
    {
        Task<IEnumerable<GoverningBodyAnnouncementUserDTO>> GetAllAnnouncementAsync();
        Task DeleteAnnouncementAsync(int id);
        Task<int?> AddAnnouncementAsync(string text);
        Task<GoverningBodyAnnouncementUserDTO> GetAnnouncementByIdAsync(int id);
        Task<List<string>> GetAllUserAsync();
        Task EditAnnouncement(int id, string text);

        /// <summary>
        /// Get specified by page number and page size list of announcements
        /// </summary>
        /// <param name="pageNumber">Number of the page</param>
        /// <param name="pageSize">Size of one page</param>
        /// <returns>Specified by page number and page size list of announcements and total amount of announcements</returns>
        Task<Tuple<IEnumerable<GoverningBodyAnnouncementUserDTO>, int>> GetAnnouncementsByPageAsync(int pageNumber, int pageSize);
    }
}
