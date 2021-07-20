using AutoMapper;
using EPlast.BLL.DTO.GoverningBody.Sector;
using EPlast.WebApi.Models.GoverningBody.Sector;

namespace EPlast.WebApi.Mapping.GoverningBody.Sector
{
    public class SectorDocumentsProfile : Profile
    {
        public SectorDocumentsProfile()
        {
            CreateMap<SectorDocumentsViewModel, SectorDocumentsDTO>().ReverseMap();
            CreateMap<SectorViewModel, SectorDocumentsDTO>().ReverseMap();
            CreateMap<SectorDocumentTypeViewModel, SectorDocumentTypeDTO>().ReverseMap();
        }
    }
}