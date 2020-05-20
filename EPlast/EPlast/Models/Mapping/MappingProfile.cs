using AutoMapper;
using EPlast.BussinessLayer.DTO.Account;
using EPlast.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.Models.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<LoginViewModel, LoginDto>();
            CreateMap<LoginDto, LoginViewModel>();
            CreateMap<RegisterViewModel, RegisterDto>();
            CreateMap<RegisterDto, RegisterViewModel>();
            CreateMap<ForgotPasswordDto, ForgotPasswordViewModel>();
            CreateMap<ForgotPasswordViewModel, ForgotPasswordDto>();
            CreateMap<ResetPasswordDto, ResetPasswordViewModel>();
            CreateMap<ResetPasswordViewModel, ResetPasswordDto>();
        }
    }
}
