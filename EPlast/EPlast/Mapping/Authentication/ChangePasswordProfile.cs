using AutoMapper;
using EPlast.BussinessLayer.DTO.Account;
using EPlast.ViewModels;

namespace EPlast.Models.Mapping
{
    public class ChangePasswordProfile : Profile
    {
        public ChangePasswordProfile()
        {
            CreateMap<ChangePasswordDto, ChangePasswordViewModel>().ReverseMap();
        }
    }
}
