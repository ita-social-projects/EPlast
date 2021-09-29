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
        Task<bool> AddAnnouncementAsync(string text);
        Task<GoverningBodyAnnouncementUserDTO> GetAnnouncementByIdAsync(int id);
        Task<List<string>> GetAllUserAsync();
        Task<int> EditAnnouncement(GoverningBodyAnnouncementUserDTO announcement);
    }
}
