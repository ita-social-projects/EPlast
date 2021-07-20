using AutoMapper;
using EPlast.WebApi.Models.GoverningBody.Sector;
using EPlast.BLL.DTO.GoverningBody.Sector;

namespace EPlast.WebApi.Mapping.GoverningBody.Sector
{
    public class SectorProfile : Profile
    {
        public SectorProfile()
        {
            CreateMap<SectorViewModel, SectorDTO>().ReverseMap();
            CreateMap<SectorProfileDTO, SectorViewModel>()
                .ForMember(r => r.Id, s => s.MapFrom(t => t.Sector.Id))
                .ForMember(r => r.Head, s => s.MapFrom(t => t.Head))
                .ForMember(r => r.Administration, s => s.MapFrom(t => t.Sector.Administration))
                .ForMember(r => r.Documents, s => s.MapFrom(t => t.Documents))
                .ForMember(r => r.Name, s => s.MapFrom(t => t.Sector.Name))
                .ForMember(r => r.PhoneNumber, s => s.MapFrom(t => t.Sector.PhoneNumber))
                .ForMember(r => r.Email, s => s.MapFrom(t => t.Sector.Email))
                .ForMember(r => r.Description, s => s.MapFrom(t => t.Sector.Description))
                .ForMember(r => r.Logo, s => s.MapFrom(t => t.Sector.Logo))
                .ForMember(r => r.AdministrationCount, s => s.MapFrom(t => t.Sector.AdministrationCount))
                .ForMember(r => r.GoverningBodyId, s => s.MapFrom(t => t.Sector.GoverningBodyId));
        }
}
}