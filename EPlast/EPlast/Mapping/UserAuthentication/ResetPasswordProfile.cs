using AutoMapper;
using EPlast.BussinessLayer.DTO.Account;
using EPlast.ViewModels;

namespace EPlast.Models.Mapping
{
    public class ResetPasswordProfile : Profile
    {
        public ResetPasswordProfile()
        {
            CreateMap<ResetPasswordViewModel, ResetPasswordDto>().ReverseMap();
        }
    }
}
