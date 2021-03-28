using AutoMapper;
using EPlast.BLL.DTO;
using EPlast.WebApi.Models.MethodicDocument;

namespace EPlast.WebApi.Mapping.MethodicDocument
{
    public class MethodicDocumentProfile : Profile
    {
        public MethodicDocumentProfile()
        {
            CreateMap<MethodicDocumentDTO, MethodicDocumentViewModel>()
                 .ForMember(dvw => dvw.Organization, dd => dd.MapFrom(f => f.GoverningBody.GoverningBodyName))
                 .ForMember(dvw => dvw.Date, dd => dd.MapFrom(f => f.Date.ToString("MM.dd.yyyy")))
                 .ReverseMap();
        }
    }
}
