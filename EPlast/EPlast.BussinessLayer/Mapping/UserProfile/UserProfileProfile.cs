using AutoMapper;
using EPlast.BussinessLayer.DTO.UserProfiles;

namespace EPlast.BussinessLayer.Mapping.UserProfile
{
    public class UserProfileProfile : Profile
    {
        public UserProfileProfile()
        {
            CreateMap<DataAccess.Entities.UserProfile, UserProfileDTO>().ReverseMap();
        }
    }
}
