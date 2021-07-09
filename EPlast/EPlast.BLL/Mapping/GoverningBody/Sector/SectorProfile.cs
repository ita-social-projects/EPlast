using AutoMapper;
using EPlast.BLL.DTO.GoverningBody.Sector;

namespace EPlast.BLL.Mapping.GoverningBody.Sector
{
    public class SectorProfile : Profile
    {
        public SectorProfile()
        {
            CreateMap<DataAccess.Entities.GoverningBody.Sector.Sector, SectorDTO>()
                .ForMember(g => g.Name, o => o.MapFrom(n => n.Name)).ReverseMap();
        }
    }
}