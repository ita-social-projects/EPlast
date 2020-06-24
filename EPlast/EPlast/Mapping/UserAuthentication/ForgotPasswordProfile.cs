using AutoMapper;
using EPlast.BusinessLogicLayer.DTO.Account;
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
