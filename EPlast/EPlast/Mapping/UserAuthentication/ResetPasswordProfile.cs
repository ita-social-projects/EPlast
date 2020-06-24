using AutoMapper;
using EPlast.BLL.DTO.Account;
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
