using AutoMapper;
using EPlast.BLL.DTO.City;
using EPlast.DataAccess.Entities;
using EPlast.ViewModels.City;

namespace EPlast.Mapping.City
{
    public class CityDocumentsProfile : Profile
    {
        public CityDocumentsProfile()
        {
            CreateMap<CityDocumentsDTO, CityDocuments>().ReverseMap();
            CreateMap<CityDocumentViewModel, CityDocumentsDTO>().ReverseMap();
        }
    }
}
