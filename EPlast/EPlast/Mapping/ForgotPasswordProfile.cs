using AutoMapper;
using EPlast.BussinessLayer.DTO.Account;
using EPlast.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.Models.Mapping
{
    public class ForgotPasswordProfile : Profile
    {
        public ForgotPasswordProfile()
        {
            CreateMap<ForgotPasswordDto, ForgotPasswordViewModel>();
            CreateMap<ForgotPasswordViewModel, ForgotPasswordDto>();
        }
    }
}
