using AutoMapper;
using EPlast.DataAccess.Entities.UserEntities;

namespace EPlast.BLL.Mapping.Distinctions
{
    class UserDistinctionProfile : Profile
    {
        public UserDistinctionProfile()
        {
            CreateMap<UserDistinction, UserDistinctionDTO>().ReverseMap();
        }
    }
}