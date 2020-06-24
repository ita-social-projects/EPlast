using AutoMapper;
using EPlast.Bussiness.DTO;

namespace EPlast.Bussiness.Mapping.ConfirmedUser
{
    public class ConfirmedUserProfile : Profile
    {
        public ConfirmedUserProfile()
        {
            CreateMap<DataAccess.Entities.ConfirmedUser, ConfirmedUserDTO>().ReverseMap();
        }
    }
}
