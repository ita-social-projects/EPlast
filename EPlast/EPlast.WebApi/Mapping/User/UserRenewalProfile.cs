using AutoMapper;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.WebApi.Models.UserModels;

namespace EPlast.WebApi.Mapping.User
{
    public class UserRenewalProfile : Profile
    {
        public UserRenewalProfile()
        {
            CreateMap<UserRenewalDto, UserRenewalViewModel>()
                .ForPath(x => x.Id, y => y.MapFrom(z => z.Id))
                .ForPath(x => x.UserId, y => y.MapFrom(z => z.UserId))
                .ForPath(x => x.CityId, y => y.MapFrom(z => z.CityId))
                .ForPath(x => x.RequestDate, y => y.MapFrom(z => z.RequestDate))
                .ForPath(x => x.Approved, y => y.MapFrom(z => z.Approved))
                .ForPath(x => x.FirstName, y => y.MapFrom(z => z.User.FirstName))
                .ForPath(x => x.SecondName, y => y.MapFrom(z => z.User.LastName))
                .ForPath(x => x.Email, y => y.MapFrom(z => z.User.Email))
                .ForPath(x => x.CityName, y => y.MapFrom(z => z.City.Name));
        }
    }
}
