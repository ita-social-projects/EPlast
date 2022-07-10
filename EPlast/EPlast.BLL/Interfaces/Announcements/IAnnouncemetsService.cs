using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.BLL.DTO.GoverningBody.Announcement;

namespace EPlast.BLL.Interfaces.Announcements
{
    public interface IAnnouncemetsService
    {
        Task<Tuple<IEnumerable<GoverningBodyAnnouncementUserDto>, int>> GetAnnouncementsByPageAsync(int pageNumber, int pageSize);
        Task<GoverningBodyAnnouncementUserWithImagesDto> GetAnnouncementByIdAsync(int id);
        Task<int?> PinAnnouncementAsync(int id);
    }
}
