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
                .ForMember(x => x.City, q => q.MapFrom(w => w.CityMembers.FirstOrDefault().City.Name))
                .ForMember(x => x.Club, q => q.MapFrom(w => w.ClubMembers.FirstOrDefault().Club.Name))
                .ForMember(x => x.PublicPoliticalActivity, q => q.MapFrom(w => w.UserProfile.PublicPoliticalActivity))
                .ForPath(x => x.UpuDegree.Name, q => q.MapFrom(w => w.UserProfile.UpuDegree.Name))
                .ForPath(x => x.UpuDegree.ID, q => q.MapFrom(w => w.UserProfile.UpuDegree.ID))
                .ReverseMap();
        }
    }
}
