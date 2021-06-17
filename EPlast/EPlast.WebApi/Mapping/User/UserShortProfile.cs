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
                .ForMember(x => x.Region, q => q.MapFrom(w => w.CityMembers.FirstOrDefault().City.Region.RegionName))
                .ForMember(x => x.City, q => q.MapFrom(w => w.CityMembers.FirstOrDefault().City.Name))
                .ForMember(x => x.Club, q => q.MapFrom(w => w.ClubMembers.FirstOrDefault(x=>x.IsApproved).Club.Name))
                .ForPath(x => x.UpuDegree.Name, q => q.MapFrom(w => w.UserProfile.UpuDegree.Name))
                .ForPath(x => x.UpuDegree.ID, q => q.MapFrom(w => w.UserProfile.UpuDegree.ID))
                .ForMember(x => x.FacebookLink, q => q.MapFrom(w => w.UserProfile.FacebookLink))
                .ForMember(x => x.TwitterLink, q => q.MapFrom(w => w.UserProfile.TwitterLink))
                .ForMember(x => x.InstagramLink, q => q.MapFrom(w => w.UserProfile.InstagramLink))
                .ForMember(x => x.CityId, q => q.MapFrom(w => w.CityMembers.FirstOrDefault().City.ID))
                .ForMember(x => x.ClubId, q => q.MapFrom(w => w.ClubMembers.FirstOrDefault(x=>x.IsApproved).Club.ID))
                .ForMember(x => x.RegionId, q => q.MapFrom(w => w.CityMembers.FirstOrDefault().City.RegionId));

            CreateMap<UserShortViewModel, UserDTO>()
                    .ForPath(x => x.UserProfile.ID, q => q.MapFrom(w => w.UserProfileID))
                    .ForPath(x => x.UserProfile.UserID, q => q.MapFrom(w => w.ID))
                    .ForPath(x => x.UserProfile.Pseudo, q => q.MapFrom(w => w.Pseudo))
                    .ForPath(x => x.UserProfile.UpuDegree.Name, q => q.MapFrom(w => w.UpuDegree.Name))
                    .ForPath(x => x.UserProfile.UpuDegree.ID, q => q.MapFrom(w => w.UpuDegree.ID))
                    .ForPath(x => x.UserProfile.FacebookLink, q => q.MapFrom(w => w.FacebookLink))
                    .ForPath(x => x.UserProfile.TwitterLink, q => q.MapFrom(w => w.TwitterLink))
                    .ForPath(x => x.UserProfile.InstagramLink, q => q.MapFrom(w => w.InstagramLink));
        }
    }
}
