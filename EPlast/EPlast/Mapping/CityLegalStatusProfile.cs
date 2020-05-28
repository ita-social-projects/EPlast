using AutoMapper;
using EPlast.BussinessLayer.DTO;
using EPlast.DataAccess.Entities;
using EPlast.ViewModels;

namespace EPlast.Mapping
{
    public class CityLegalStatusProfile : Profile
    {
        public CityLegalStatusProfile()
        {
            CreateMap<CityLegalStatus, CityLegalStatusDTO>().ReverseMap();
            CreateMap<CityLegalStatusViewModel, CityLegalStatusDTO>().ReverseMap();
        }
    }
}