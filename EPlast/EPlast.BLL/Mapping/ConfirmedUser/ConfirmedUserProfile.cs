using AutoMapper;
using EPlast.BLL.DTO;

namespace EPlast.BLL.Mapping.ConfirmedUser
{
    public class ConfirmedUserProfile : Profile
    {
        public ConfirmedUserProfile()
        {
            CreateMap<DataAccess.Entities.ConfirmedUser, ConfirmedUserDto>().ReverseMap();
        }
    }
}
