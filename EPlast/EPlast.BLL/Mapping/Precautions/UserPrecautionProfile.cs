using AutoMapper;
using EPlast.DataAccess.Entities.UserEntities;

namespace EPlast.BLL.Mapping.Precautions
{
    class UserPrecautionProfile : Profile
    {
        public UserPrecautionProfile()
        {
            CreateMap<UserPrecaution, UserPrecautionDTO>().ReverseMap();
            CreateMap<UserPrecaution, UserPrecautionsTableObject>().ForMember(dest => dest.PrecautionName, act => act.MapFrom(src => src.Precaution.Name))
                    .ForMember(dest => dest.UserName, act => act.MapFrom(src => src.User.FirstName+" "+src.User.LastName));
        }
    }
}
