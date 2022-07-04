using AutoMapper;
using EPlast.BLL.DTO;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BLL.Mapping.GoverningBody
{
    public class GoverningBodyProfile : Profile
    {
        public GoverningBodyProfile()
        {
            CreateMap<DatabaseEntities.GoverningBody.Organization, GoverningBodyDto>()
                .ForMember(g => g.GoverningBodyName, o => o.MapFrom(n => n.OrganizationName))
                .ForMember(g => g.GoverningBodyAnnouncements, o => o.MapFrom(n => n.GoverningBodyAnnouncement)).ReverseMap();
        }
    }
}
