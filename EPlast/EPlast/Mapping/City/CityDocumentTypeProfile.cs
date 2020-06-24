using AutoMapper;
using EPlast.BusinessLogicLayer.DTO.City;
using EPlast.DataAccess.Entities;
using EPlast.ViewModels.City;

namespace EPlast.Mapping.City
{
    public class CityDocumentTypeProfile : Profile
    {
        public CityDocumentTypeProfile()
        {
            CreateMap<CityDocumentTypeDTO, CityDocumentType>().ReverseMap();
            CreateMap<CityDocumentTypeViewModel, CityDocumentTypeDTO>().ReverseMap();
        }
    }
}
