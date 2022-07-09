using AutoMapper;
using EPlast.BLL.DTO;
using EPlast.WebApi.Models.Approver;

namespace EPlast.WebApi.Mapping.Approver
{
    public class ConfirmedUserProfile : Profile
    {
        public ConfirmedUserProfile()
        {
            CreateMap<ConfirmedUserDto, ConfirmedUserViewModel>().ReverseMap();
        }
    }
}
