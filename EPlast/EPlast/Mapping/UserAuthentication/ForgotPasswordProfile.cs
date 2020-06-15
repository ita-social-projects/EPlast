using AutoMapper;
using EPlast.BussinessLayer.DTO.Account;
using EPlast.ViewModels;

namespace EPlast.Models.Mapping
{
    public class ForgotPasswordProfile : Profile
    {
        public ForgotPasswordProfile()
        {
            CreateMap<ForgotPasswordDTO, ForgotPasswordViewModel>().ReverseMap();
        }
    }
}
