using AutoMapper;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.WebApi.Models.User;
using System.Linq;

namespace EPlast.WebApi.Mapping.User
{
    public class UserInfoProfile : Profile
    {
        public UserInfoProfile()
        {
            CreateMap<EPlast.DataAccess.Entities.User, UserDTO>().ReverseMap();
            CreateMap<UserDTO, UserInfoViewModel>()
                .ForMember(x => x.Pseudo, q => q.MapFrom(w => w.UserProfile.Pseudo))
                .ForMember(x => x.City, q => q.MapFrom(w => w.CityMembers.FirstOrDefault().City.Name))
                .ForMember(x => x.Club, q => q.MapFrom(w => w.ClubMembers.FirstOrDefault().Club.Name))
                .ReverseMap();
        }
    }
}