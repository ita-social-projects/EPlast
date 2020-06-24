using AutoMapper;
using EPlast.BLL.DTO.Account;
using EPlast.ViewModels;

namespace EPlast.Models.Mapping
{
    public class ForgotPasswordProfile : Profile
    {
        public ForgotPasswordProfile()
        {
            CreateMap<ForgotPasswordDto, ForgotPasswordViewModel>().ReverseMap();
        }
    }
}
