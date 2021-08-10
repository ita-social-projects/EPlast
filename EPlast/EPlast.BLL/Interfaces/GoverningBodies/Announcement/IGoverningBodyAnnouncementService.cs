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
        Task DeleteAnnouncement(int id);
        Task<bool> AddAnnouncement(string text);
        Task<GoverningBodyAnnouncementUserDTO> GetAnnouncementById(int id);
        Task<List<string>> GetAllUserAsync();
    }
}
