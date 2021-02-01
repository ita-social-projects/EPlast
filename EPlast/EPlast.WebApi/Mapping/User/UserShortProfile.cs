using AutoMapper;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.WebApi.Models.UserModels;
using System.Linq;

namespace EPlast.WebApi.Mapping.User
{
    public class UserShortProfile : Profile
    {
        public UserShortProfile()
        {
            CreateMap<UserDTO, UserShortViewModel>()
                .ForMember(x => x.UserProfileID, q => q.MapFrom(w => w.UserProfile.ID))
                .ForMember(x => x.ID, q => q.MapFrom(w => w.UserProfile.UserID))
                .ForMember(x => x.Pseudo, q => q.MapFrom(w => w.UserProfile.Pseudo))
                .ForMember(x => x.City, q => q.MapFrom(w => w.CityMembers.FirstOrDefault().City.Name))
                .ForMember(x => x.Club, q => q.MapFrom(w => w.ClubMembers.FirstOrDefault().Club.Name))
                .ForPath(x => x.UpuDegree.Name, q => q.MapFrom(w => w.UserProfile.UpuDegree.Name))
                .ForPath(x => x.UpuDegree.ID, q => q.MapFrom(w => w.UserProfile.UpuDegree.ID))
                .ForMember(x => x.FacebookLink, q => q.MapFrom(w => w.UserProfile.FacebookLink))
                .ForMember(x => x.TwitterLink, q => q.MapFrom(w => w.UserProfile.TwitterLink))
                .ForMember(x => x.InstagramLink, q => q.MapFrom(w => w.UserProfile.InstagramLink))
                .ReverseMap();
        }
    }
}
