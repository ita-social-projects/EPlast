using AutoMapper;
using EPlast.BLL.DTO.GoverningBody;
using EPlast.WebApi.Models.GoverningBody;

namespace EPlast.WebApi.Mapping.GoverningBody
{
    public class GoverningBodyDocumentsProfile : Profile
    {
        public GoverningBodyDocumentsProfile()
        {
            CreateMap<GoverningBodyDocumentsViewModel, GoverningBodyDocumentsDTO>().ReverseMap();
            CreateMap<GoverningBodyViewModel, GoverningBodyDocumentsDTO>().ReverseMap();
            CreateMap<GoverningBodyDocumentTypeViewModel, GoverningBodyDocumentTypeDTO>().ReverseMap();
        }
    }
}
