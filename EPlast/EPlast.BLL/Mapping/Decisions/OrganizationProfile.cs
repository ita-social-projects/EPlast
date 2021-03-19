using AutoMapper;
using EPlast.BLL.DTO;
using EPlast.DataAccess.Entities;

namespace EPlast.BLL.Mapping
{
    public class OrganizationProfile : Profile
    {
        public OrganizationProfile()
        {
            CreateMap<DataAccess.Entities.GoverningBody, GoverningBodyDTO>()
                .ForMember("Name", options =>
                options.MapFrom(org => org.GoverningBodyName))
                .ReverseMap();
        }
    }
}