using AutoMapper;
using EPlast.BLL.DTO.GoverningBody.Sector;
using EPlast.DataAccess.Entities.GoverningBody.Sector;

namespace EPlast.BLL.Mapping.GoverningBody.Sector
{
    public class SectorAdministrationProfile : Profile
    {
        public SectorAdministrationProfile()
        {
            CreateMap<SectorAdministration, SectorAdministrationDto>().ReverseMap();
        }
    }
}