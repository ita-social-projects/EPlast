using EPlast.BLL.DTO.GoverningBody.Announcement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.Announcements
{
    public interface IAnnouncemetsService
    {
        Task<Tuple<IEnumerable<GoverningBodyAnnouncementUserDTO>, int>> GetAnnouncementsByPageAsync(int pageNumber, int pageSize);
        Task<GoverningBodyAnnouncementUserWithImagesDTO> GetAnnouncementByIdAsync(int id);
        Task<int?> PinAnnouncementAsync(int id);
    }
}
