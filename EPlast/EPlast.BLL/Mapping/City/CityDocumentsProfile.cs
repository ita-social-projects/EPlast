using AutoMapper;
using EPlast.BLL.DTO.City;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BLL.Mapping.City
{
    public class CityDocumentsProfile : Profile
    {
        public CityDocumentsProfile()
        {
            CreateMap<DatabaseEntities.CityDocuments, CityDocumentsDTO>().ReverseMap();
            CreateMap<DatabaseEntities.CityDocumentType, CityDocumentTypeDTO>().ReverseMap();
        }
    }
}
