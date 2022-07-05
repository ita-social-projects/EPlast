using AutoMapper;
using EPlast.BLL.DTO.City;
using EPlast.WebApi.Models.City;

namespace EPlast.BLL.Mapping.City
{
    public class CityDocumentsProfile : Profile
    {
        public CityDocumentsProfile()
        {
            CreateMap<CityDocumentsViewModel, CityDocumentsDto>().ReverseMap();
            CreateMap<CityDocumentTypeViewModel, CityDocumentTypeDto>().ReverseMap();
        }
    }
}
