using AutoMapper;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.WebApi.Models.UserModels;

namespace EPlast.WebApi.Mapping.User
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserDTO, UserViewModel>()
                .ForMember(x => x.PlaceOfStudy, q => q.MapFrom(w => w.UserProfile.Education.PlaceOfStudy))
                .ForMember(x => x.Speciality, q => q.MapFrom(w => w.UserProfile.Education.Speciality))
                .ForMember(x => x.NationalityName, q => q.MapFrom(w => w.UserProfile.Nationality.Name))
                .ForMember(x => x.ReligionName, q => q.MapFrom(w => w.UserProfile.Religion.Name))
                .ForMember(x => x.DegreeName, q => q.MapFrom(w => w.UserProfile.Degree.Name))
                .ForMember(x => x.PlaceOfWork, q => q.MapFrom(w => w.UserProfile.Work.PlaceOfwork))
                .ForMember(x => x.PositionOfWork, q => q.MapFrom(w => w.UserProfile.Work.Position))
                .ForMember(x => x.Address, q => q.MapFrom(w => w.UserProfile.Address))
                .ReverseMap();
        }
    }
}
