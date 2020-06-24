using AutoMapper;
using EPlast.BusinessLogicLayer.DTO;

namespace EPlast.BusinessLogicLayer.Mapping.ConfirmedUser
{
    public class ConfirmedUserProfile : Profile
    {
        public ConfirmedUserProfile()
        {
            CreateMap<DataAccess.Entities.ConfirmedUser, ConfirmedUserDTO>().ReverseMap();
        }
    }
}
