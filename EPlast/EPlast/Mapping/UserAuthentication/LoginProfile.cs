using AutoMapper;
using EPlast.BusinessLogicLayer.DTO.Account;
using EPlast.ViewModels;

namespace EPlast.Models.Mapping
{
    public class LoginProfile : Profile
    {
        public LoginProfile()
        {
            CreateMap<LoginViewModel, LoginDto>().ReverseMap();
        }
    }
}
