using System.Linq;
using AutoMapper;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.WebApi.Models.User;

namespace EPlast.WebApi.Mapping.User
{
    public class UserInfoProfile : Profile
    {
        public UserInfoProfile()
        {
            CreateMap<EPlast.DataAccess.Entities.User, UserDto>().ReverseMap();
            CreateMap<UserDto, UserInfoViewModel>()
                .ForMember(x => x.Pseudo, q => q.MapFrom(w => w.UserProfile.Pseudo))
                .ForMember(x => x.Region, q => q.MapFrom(w => w.CityMembers.FirstOrDefault().City.Region.RegionName))
                .ForMember(x => x.City, q => q.MapFrom(w => w.CityMembers.FirstOrDefault().City.Name))
                .ForMember(x => x.Club, q => q.MapFrom(w => w.ClubMembers.FirstOrDefault().Club.Name))
                .ForMember(x => x.CityId, q => q.MapFrom(w => w.CityMembers.FirstOrDefault().City.ID))
                .ForMember(x => x.ClubId, q => q.MapFrom(w => w.ClubMembers.FirstOrDefault().Club.ID))
                .ForMember(x => x.RegionId, q => q.MapFrom(w => w.CityMembers.FirstOrDefault().City.RegionId))
                .ReverseMap();
        }
    }
}