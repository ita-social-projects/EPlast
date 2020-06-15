using AutoMapper;
using EPlast.BussinessLayer.DTO.Account;
using EPlast.WebApi.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.WebApi.Mapping.UserAuthentication
{
    public class LoginProfile : Profile
    {
        public LoginProfile()
        {
            CreateMap<LoginViewModel, LoginDto>().ReverseMap();
        }
    }
}
