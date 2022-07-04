using AutoMapper;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.DataAccess.Entities;

namespace EPlast.BLL.Mapping.UserProfile
{
    public class UserRenewalProfile : Profile
    {
        public UserRenewalProfile()
        {
            CreateMap<UserRenewal, UserRenewalDto>().ReverseMap();
        }
    }
}
