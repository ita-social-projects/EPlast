using AutoMapper;
using EPlast.BLL.DTO.GoverningBody.Sector;
using EPlast.DataAccess.Entities.GoverningBody.Sector;

namespace EPlast.BLL.Mapping.GoverningBody.Sector
{
    public class SectorDocumentsProfile : Profile
    {
        public SectorDocumentsProfile()
        {
            CreateMap<SectorDocuments, SectorDocumentsDto>().ReverseMap();
            CreateMap<SectorDocumentType, SectorDocumentTypeDto>().ReverseMap();
        }
    }
};