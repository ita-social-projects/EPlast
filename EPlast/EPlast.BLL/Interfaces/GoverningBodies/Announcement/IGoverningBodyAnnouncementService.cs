using EPlast.BLL.DTO.GoverningBody.Announcement;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.GoverningBodies
{
    public interface IGoverningBodyAnnouncementService
    {
        Task DeleteAnnouncementAsync(int id);
        Task<int?> AddAnnouncementAsync(GoverningBodyAnnouncementWithImagesDTO announcementDTO);
        Task<GoverningBodyAnnouncementUserDTO> GetAnnouncementByIdAsync(int id);
        Task<List<string>> GetAllUserAsync();
        Task<int?> EditAnnouncementAsync(GoverningBodyAnnouncementWithImagesDTO announcementDTO);

        /// <summary>
        /// Get specified by page number and page size list of announcements
        /// </summary>
        /// <param name="pageNumber">Number of the page</param>
        /// <param name="pageSize">Size of one page</param>
        /// <returns>Specified by page number and page size list of announcements and total amount of announcements</returns>
        Task<Tuple<IEnumerable<GoverningBodyAnnouncementUserDTO>, int>> GetAnnouncementsByPageAsync(int pageNumber, int pageSize);
    }
}
