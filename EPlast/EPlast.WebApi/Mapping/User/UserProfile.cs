using AutoMapper;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.WebApi.Models.UserModels;
using System.Linq;
namespace EPlast.WebApi.Mapping.User
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserDTO, UserViewModel>()
                .ForMember(x => x.UserProfileID, q => q.MapFrom(w => w.UserProfile.ID))
                .ForMember(x => x.ID, q => q.MapFrom(w => w.UserProfile.UserID))
                .ForPath(x => x.Nationality.Name, q => q.MapFrom(w => w.UserProfile.Nationality.Name))
                .ForPath(x => x.Nationality.ID, q => q.MapFrom(w => w.UserProfile.Nationality.ID))
                .ForPath(x => x.Religion.Name, q => q.MapFrom(w => w.UserProfile.Religion.Name))
                .ForPath(x => x.Religion.ID, q => q.MapFrom(w => w.UserProfile.Religion.ID))
                .ForPath(x => x.Degree.Name, q => q.MapFrom(w => w.UserProfile.Degree.Name))
                .ForPath(x => x.Degree.ID, q => q.MapFrom(w => w.UserProfile.Degree.ID))
                .ForPath(x => x.Education.ID, q => q.MapFrom(w => w.UserProfile.Education.ID))
                .ForPath(x => x.Education.PlaceOfStudy, q => q.MapFrom(w => w.UserProfile.Education.PlaceOfStudy))
                .ForPath(x => x.Education.Speciality, q => q.MapFrom(w => w.UserProfile.Education.Speciality))
                .ForPath(x => x.Work.ID, q => q.MapFrom(w => w.UserProfile.Work.ID))
                .ForPath(x => x.Work.PlaceOfwork, q => q.MapFrom(w => w.UserProfile.Work.PlaceOfwork))
                .ForPath(x => x.Work.Position, q => q.MapFrom(w => w.UserProfile.Work.Position))
                .ForMember(x => x.Address, q => q.MapFrom(w => w.UserProfile.Address))
                .ForPath(x => x.Gender.Name, q => q.MapFrom(w => w.UserProfile.Gender.Name))
                .ForPath(x => x.Gender.ID, q => q.MapFrom(w => w.UserProfile.Gender.ID))
                .ForMember(x => x.Birthday, q => q.MapFrom(w => w.UserProfile.Birthday))
                .ForMember(x => x.Pseudo, q => q.MapFrom(w => w.UserProfile.Pseudo))
                .ForMember(x => x.Region, q => q.MapFrom(w => w.CityMembers.FirstOrDefault().City.Region.RegionName))
                .ForMember(x => x.City, q => q.MapFrom(w => w.CityMembers.FirstOrDefault().City.Name))
                .ForMember(x => x.Club, q => q.MapFrom(w => w.ClubMembers.FirstOrDefault(x => x.IsApproved).Club.Name))
                .ForMember(x => x.PublicPoliticalActivity, q => q.MapFrom(w => w.UserProfile.PublicPoliticalActivity))
                .ForPath(x => x.UpuDegree.Name, q => q.MapFrom(w => w.UserProfile.UpuDegree.Name))
                .ForPath(x => x.UpuDegree.ID, q => q.MapFrom(w => w.UserProfile.UpuDegree.ID))
                .ForMember(x => x.FacebookLink, q => q.MapFrom(w => w.UserProfile.FacebookLink))
                .ForMember(x => x.TwitterLink, q => q.MapFrom(w => w.UserProfile.TwitterLink))
                .ForMember(x => x.InstagramLink, q => q.MapFrom(w => w.UserProfile.InstagramLink))
                .ForMember(x => x.CityId, q => q.MapFrom(w => w.CityMembers.FirstOrDefault().City.ID))
                .ForMember(x => x.ClubId, q => q.MapFrom(w => w.ClubMembers.FirstOrDefault(x=>x.IsApproved).Club.ID))
                .ForMember(x => x.RegionId, q => q.MapFrom(w => w.CityMembers.FirstOrDefault().City.RegionId));

            CreateMap<UserViewModel, UserDTO>()
                    .ForPath(x => x.UserProfile.ID, q => q.MapFrom(w => w.UserProfileID))
                    .ForPath(x => x.UserProfile.UserID, q => q.MapFrom(w => w.ID))
                    .ForPath(x => x.UserProfile.Nationality.Name, q => q.MapFrom(w => w.Nationality.Name))
                    .ForPath(x => x.UserProfile.Nationality.ID, q => q.MapFrom(w => w.Nationality.ID))
                    .ForPath(x => x.UserProfile.Religion.Name, q => q.MapFrom(w => w.Religion.Name))
                    .ForPath(x => x.UserProfile.Religion.ID, q => q.MapFrom(w => w.Religion.ID))
                    .ForPath(x => x.UserProfile.Degree.Name, q => q.MapFrom(w => w.Degree.Name))
                    .ForPath(x => x.UserProfile.Degree.ID, q => q.MapFrom(w => w.Degree.ID))
                    .ForPath(x => x.UserProfile.Education.ID, q => q.MapFrom(w => w.Education.ID))
                    .ForPath(x => x.UserProfile.Education.PlaceOfStudy, q => q.MapFrom(w => w.Education.PlaceOfStudy))
                    .ForPath(x => x.UserProfile.Education.Speciality, q => q.MapFrom(w => w.Education.Speciality))
                    .ForPath(x => x.UserProfile.Work.ID, q => q.MapFrom(w => w.Work.ID))
                    .ForPath(x => x.UserProfile.Work.PlaceOfwork, q => q.MapFrom(w => w.Work.PlaceOfwork))
                    .ForPath(x => x.UserProfile.Work.Position, q => q.MapFrom(w => w.Work.Position))
                    .ForPath(x => x.UserProfile.Address, q => q.MapFrom(w => w.Address))
                    .ForPath(x => x.UserProfile.Gender.Name, q => q.MapFrom(w => w.Gender.Name))
                    .ForPath(x => x.UserProfile.Gender.ID, q => q.MapFrom(w => w.Gender.ID))
                    .ForPath(x => x.UserProfile.Birthday, q => q.MapFrom(w => w.Birthday))
                    .ForPath(x => x.UserProfile.Pseudo, q => q.MapFrom(w => w.Pseudo))
                    .ForPath(x => x.UserProfile.PublicPoliticalActivity, q => q.MapFrom(w => w.PublicPoliticalActivity))
                    .ForPath(x => x.UserProfile.UpuDegree.Name, q => q.MapFrom(w => w.UpuDegree.Name))
                    .ForPath(x => x.UserProfile.UpuDegree.ID, q => q.MapFrom(w => w.UpuDegree.ID))
                    .ForPath(x => x.UserProfile.FacebookLink, q => q.MapFrom(w => w.FacebookLink))
                    .ForPath(x => x.UserProfile.TwitterLink, q => q.MapFrom(w => w.TwitterLink))
                    .ForPath(x => x.UserProfile.InstagramLink, q => q.MapFrom(w => w.InstagramLink));
        }
    }
}