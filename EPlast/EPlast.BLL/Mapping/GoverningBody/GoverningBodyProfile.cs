using AutoMapper;
using EPlast.BLL.DTO;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BLL.Mapping.GoverningBody
{
    public class GoverningBodyProfile : Profile
    {
        public GoverningBodyProfile()
        {
            CreateMap<DatabaseEntities.GoverningBody.Organization, GoverningBodyDTO>()
                .ForMember(g => g.GoverningBodyName, o => o.MapFrom(n => n.OrganizationName)).ReverseMap();
        }
    }
}
