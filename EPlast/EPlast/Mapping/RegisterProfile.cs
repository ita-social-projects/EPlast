using AutoMapper;
using EPlast.BussinessLayer.DTO.Account;
using EPlast.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.Models.Mapping
{
    public class RegisterProfile : Profile
    {
        public RegisterProfile()
        {
            CreateMap<RegisterViewModel, RegisterDto>();
            CreateMap<RegisterDto, RegisterViewModel>();
        }
    }
}
