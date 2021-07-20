using AutoMapper;
using EPlast.BLL.DTO;
using EPlast.BLL.DTO.ActiveMembership;
using EPlast.BLL.DTO.GoverningBody;
using EPlast.WebApi.Models.GoverningBody;

namespace EPlast.WebApi.Mapping.GoverningBody
{
    public class GoverningBodyProfile : Profile
    {
        public GoverningBodyProfile()
        {
            CreateMap<GoverningBodyViewModel, GoverningBodyDTO>().ReverseMap();
            CreateMap<GoverningBodyProfileDTO, GoverningBodyViewModel>()
                .ForMember(r => r.Id, s => s.MapFrom(t => t.GoverningBody.Id))
                .ForMember(r => r.Head, s => s.MapFrom(t => t.Head))
                .ForMember(r => r.Administration, s => s.MapFrom(t => t.GoverningBodyAdministration))
                .ForMember(r => r.Documents, s => s.MapFrom(t => t.Documents))
                .ForMember(r => r.GoverningBodyName, s => s.MapFrom(t => t.GoverningBody.GoverningBodyName))
                .ForMember(r => r.PhoneNumber, s => s.MapFrom(t => t.GoverningBody.PhoneNumber))
                .ForMember(r => r.Email, s => s.MapFrom(t => t.GoverningBody.Email))
                .ForMember(r => r.Description, s => s.MapFrom(t => t.GoverningBody.Description))
                .ForMember(r => r.Logo, s => s.MapFrom(t => t.GoverningBody.Logo))
                .ForMember(r => r.AdministrationCount, s => s.MapFrom(t => t.GoverningBody.AdministrationCount))
                .ForMember(r => r.Sectors, s => s.MapFrom(t => t.Sectors));
        }
    }
}
