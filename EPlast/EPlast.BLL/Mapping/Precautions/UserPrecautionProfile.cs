using AutoMapper;
using EPlast.DataAccess.Entities.UserEntities;

namespace EPlast.BLL.Mapping.Precautions
{
    class UserPrecautionProfile : Profile
    {
        public UserPrecautionProfile()
        {
            CreateMap<UserPrecaution, UserPrecautionDTO>().ReverseMap();
        }
    }
}
