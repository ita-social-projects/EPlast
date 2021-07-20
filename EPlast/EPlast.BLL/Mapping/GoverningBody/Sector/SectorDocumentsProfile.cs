using AutoMapper;
using EPlast.BLL.DTO.GoverningBody.Sector;
using EPlast.DataAccess.Entities.GoverningBody.Sector;

namespace EPlast.BLL.Mapping.GoverningBody.Sector
{
    public class SectorDocumentsProfile : Profile
    {
        public SectorDocumentsProfile()
        {
            CreateMap<SectorDocuments, SectorDocumentsDTO>().ReverseMap();
            CreateMap<SectorDocumentType, SectorDocumentTypeDTO>().ReverseMap();
        }
    }
};