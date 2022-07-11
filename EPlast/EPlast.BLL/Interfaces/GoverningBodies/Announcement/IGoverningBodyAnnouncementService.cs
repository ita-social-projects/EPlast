using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EPlast.BLL.DTO.GoverningBody.Announcement;

namespace EPlast.BLL.Interfaces.GoverningBodies
{
    public interface IGoverningBodyAnnouncementService
    {
        Task DeleteAnnouncementAsync(int id);
        Task<int?> AddAnnouncementAsync(GoverningBodyAnnouncementWithImagesDto announcementDTO);
        Task<GoverningBodyAnnouncementUserWithImagesDto> GetAnnouncementByIdAsync(int id);
        Task<List<string>> GetAllUserAsync();
        Task<int?> EditAnnouncementAsync(GoverningBodyAnnouncementWithImagesDto announcementDTO);

        /// <summary>
        /// Get specified by page number and page size list of announcements
        /// </summary>
        /// <param name="pageNumber">Number of the page</param>
        /// <param name="pageSize">Size of one page</param>
        /// <param name="governingBodyId">Id of governing body</param>
        /// <returns>Specified by page number and page size list of announcements and total amount of announcements</returns>
        Task<Tuple<IEnumerable<GoverningBodyAnnouncementUserDto>, int>> GetAnnouncementsByPageAsync(int pageNumber, int pageSize, int governingBodyId);
    }
}
