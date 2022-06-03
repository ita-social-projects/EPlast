using AutoMapper;
using EPlast.BLL.DTO.GoverningBody.Announcement;
using EPlast.DataAccess.Entities.GoverningBody;
using EPlast.DataAccess.Entities.GoverningBody.Announcement;

namespace EPlast.BLL.Mapping.GoverningBody.Announcement
{
    public class GoverningBodyAnnouncementProfile : Profile
    {
        public GoverningBodyAnnouncementProfile()
        {
            CreateMap<GoverningBodyAnnouncement, GoverningBodyAnnouncementDTO>().ReverseMap();
        }
    }
}
