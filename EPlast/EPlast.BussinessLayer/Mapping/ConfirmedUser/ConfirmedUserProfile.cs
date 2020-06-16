using AutoMapper;
using EPlast.BussinessLayer.DTO;

namespace EPlast.BussinessLayer.Mapping.ConfirmedUser
{
    public class ConfirmedUserProfile : Profile
    {
        public ConfirmedUserProfile()
        {
            CreateMap<DataAccess.Entities.ConfirmedUser, ConfirmedUserDTO>().ReverseMap();
        }
    }
}
