using AutoMapper;
using EPlast.BLL.DTO;

namespace EPlast.BLL.Mapping
{
    public class OrganizationProfile : Profile
    {
        public OrganizationProfile()
        {
            CreateMap<DataAccess.Entities.GoverningBody.Organization, GoverningBodyDTO>().ForMember("GoverningBodyName",
                    opt => opt.MapFrom(c  => c.OrganizationName))
                .ReverseMap();
        }
    }
}
