using AutoMapper;
using EPlast.BLL.DTO;
using EPlast.DataAccess.Entities;

namespace EPlast.BLL.Mapping
{
    public class OrganizationProfile : Profile
    {
        public OrganizationProfile()
        {
            CreateMap<DataAccess.Entities.Organization, GoverningBodyDTO>().ForMember("GoverningBodyName",
                    opt => opt.MapFrom(c  => c.OrganizationName))
                .ReverseMap();
        }
    }
}
