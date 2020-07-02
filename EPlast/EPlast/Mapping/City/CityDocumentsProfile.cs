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
            CreateMap<CityDocumentViewModel, CityDocumentsDTO>().ReverseMap();
            CreateMap<CityDocumentTypeViewModel, CityDocumentTypeDTO>().ReverseMap();
        }
    }
}
