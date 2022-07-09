using AutoMapper;
using EPlast.BLL.DTO.City;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BLL.Mapping.City
{
    public class CityDocumentsProfile : Profile
    {
        public CityDocumentsProfile()
        {
            CreateMap<DatabaseEntities.CityDocuments, CityDocumentsDto>().ReverseMap();
            CreateMap<DatabaseEntities.CityDocumentType, CityDocumentTypeDto>().ReverseMap();
        }
    }
}
