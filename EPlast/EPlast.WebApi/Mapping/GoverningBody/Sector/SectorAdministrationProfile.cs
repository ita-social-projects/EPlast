using AutoMapper;
using EPlast.BLL.DTO.GoverningBody.Sector;
using EPlast.WebApi.Models.GoverningBody.Sector;

namespace EPlast.WebApi.Mapping.GoverningBody.Sector
{
    public class SectorAdministrationProfile : Profile
    {
        public SectorAdministrationProfile()
        {
            CreateMap<SectorAdministrationViewModel, SectorAdministrationDTO>().ReverseMap();
        }
    }
}