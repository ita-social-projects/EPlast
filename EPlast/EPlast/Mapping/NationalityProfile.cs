using AutoMapper;
using EPlast.BussinessLayer.DTO;
using EPlast.DataAccess.Entities;
using EPlast.ViewModels.UserInformation.UserProfile;

namespace EPlast.Mapping
{
    public class NationalityProfile:Profile
    {
        public NationalityProfile()
        {
            CreateMap<Nationality, NationalityDTO>().ReverseMap();
            CreateMap<NationalityViewModel, NationalityDTO>().ReverseMap();
        }
    }
}
