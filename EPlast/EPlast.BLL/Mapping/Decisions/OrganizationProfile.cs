using AutoMapper;
using EPlast.BLL.DTO;
using EPlast.DataAccess.Entities;

namespace EPlast.BLL.Mapping
{
    public class OrganizationProfile : Profile
    {
        public OrganizationProfile()
        {
            CreateMap<DataAccess.Entities.Organization, GoverningBodyDTO>()
                .ForMember("Name", options =>
                options.MapFrom(org => org.OrganizationName))
                .ReverseMap();
        }
    }
}