using AutoMapper;
using EPlast.BLL.DTO.GoverningBody.Announcement;
using EPlast.WebApi.Models.GoverningBody;

namespace EPlast.WebApi.Mapping.GoverningBody
{
    public class GoverningBodyAnnouncementrsProfile : Profile
    {
        public GoverningBodyAnnouncementrsProfile()
        {
            CreateMap<GoverningBodyAnnouncementsViewModel, GoverningBodyAnnouncementDTO>().ReverseMap();
            CreateMap<GoverningBodyViewModel, GoverningBodyAnnouncementDTO>().ReverseMap();
        }
    }
}
