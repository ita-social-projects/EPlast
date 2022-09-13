using AutoMapper;
using EPlast.BLL.DTO.GoverningBody.Announcement;
using EPlast.WebApi.Models.GoverningBody;

namespace EPlast.WebApi.Mapping.GoverningBody
{
    public class GoverningBodyAnnouncementrsProfile : Profile
    {
        public GoverningBodyAnnouncementrsProfile()
        {
            CreateMap<GoverningBodyAnnouncementsViewModel, GoverningBodyAnnouncementDto>().ReverseMap();
            CreateMap<GoverningBodyViewModel, GoverningBodyAnnouncementDto>().ReverseMap();
        }
    }
}
