using AutoMapper;
using EPlast.BLL.DTO.GoverningBody.Sector;
using EPlast.WebApi.Models.GoverningBody.Sector;

namespace EPlast.WebApi.Mapping.GoverningBody.Sector
{
    public class SectorDocumentsProfile : Profile
    {
        public SectorDocumentsProfile()
        {
            CreateMap<SectorDocumentsViewModel, SectorDocumentsDto>().ReverseMap();
            CreateMap<SectorViewModel, SectorDocumentsDto>().ReverseMap();
            CreateMap<SectorDocumentTypeViewModel, SectorDocumentTypeDto>().ReverseMap();
        }
    }
}