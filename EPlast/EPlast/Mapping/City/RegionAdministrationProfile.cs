using AutoMapper;
using EPlast.BusinessLogicLayer.DTO;
using EPlast.DataAccess.Entities;

namespace EPlast.Mapping
{
    public class RegionAdministrationProfile : Profile
    {
        public RegionAdministrationProfile()
        {
            CreateMap<RegionAdministration, RegionAdministrationDTO>().ReverseMap();
        }
    }
}