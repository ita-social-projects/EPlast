using AutoMapper;
using EPlast.DataAccess.Entities.UserEntities;

namespace EPlast.BLL.Mapping.Distinctions
{
    class UserDistinctionProfile : Profile
    {
        public UserDistinctionProfile()
        {
            CreateMap<UserDistinction, UserDistinctionDTO>().ReverseMap();
            CreateMap<UserDistinction, UserDistinctionsTableObject>().ForMember(dest => dest.DistinctionName, act => act.MapFrom(src => src.Distinction.Name))
                    .ForMember(dest => dest.UserName, act => act.MapFrom(src => src.User.FirstName + " " + src.User.LastName));
        }
    }
}