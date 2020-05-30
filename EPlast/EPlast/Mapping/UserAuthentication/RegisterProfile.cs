using AutoMapper;
using EPlast.BussinessLayer.DTO.Account;
using EPlast.ViewModels;

namespace EPlast.Models.Mapping
{
    public class RegisterProfile : Profile
    {
        public RegisterProfile()
        {
            CreateMap<RegisterViewModel, RegisterDto>().ReverseMap();
        }
    }
}
