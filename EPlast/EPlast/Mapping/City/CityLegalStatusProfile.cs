using AutoMapper;
using EPlast.BLL.DTO.City;
using EPlast.ViewModels.City;

namespace EPlast.Mapping.City
{
    public class CityLegalStatusProfile: Profile
    {
        public CityLegalStatusProfile()
        {
            CreateMap<CityLegalStatusViewModel, CityLegalStatusDTO>().ReverseMap();
        }
    }
}
