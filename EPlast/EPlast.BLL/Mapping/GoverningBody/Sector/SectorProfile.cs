using AutoMapper;
using EPlast.BLL.DTO.GoverningBody.Sector;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BLL.Mapping.GoverningBody.Sector
{
    public class SectorProfile : Profile
    {
        public SectorProfile()
        {
            CreateMap<DatabaseEntities.GoverningBody.Sector.Sector, SectorDTO>()
                .ForMember(g => g.Name, o => o.MapFrom(n => n.Name))
                .ForMember(g => g.GoverningBodyId, o => o.MapFrom(n => n.GoverningBodyId))
                .ForMember(g => g.Announcements, o => o.MapFrom(n => n.Announcements))
                .ForMember(g => g.Administration, o => o.MapFrom(n => n.Administration))
                .ReverseMap();
        }
    }
}