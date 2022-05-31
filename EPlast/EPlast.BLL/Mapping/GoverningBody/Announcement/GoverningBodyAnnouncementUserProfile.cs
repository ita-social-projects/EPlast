using AutoMapper;
using EPlast.BLL.DTO.GoverningBody.Announcement;
using EPlast.DataAccess.Entities.GoverningBody;
using EPlast.DataAccess.Entities.GoverningBody.Announcement;

namespace EPlast.BLL.Mapping.GoverningBody.Announcement
{
    public class GoverningBodyAnnouncementUserProfile : Profile
    {
        public GoverningBodyAnnouncementUserProfile()
        {
            CreateMap<GoverningBodyAnnouncement, GoverningBodyAnnouncementUserDTO>().ReverseMap();
        }
    }
}
