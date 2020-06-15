using AutoMapper;
using EPlast.BussinessLayer.DTO.Account;
using EPlast.WebApi.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.WebApi.Mapping.UserAuthentication
{
    public class RegisterProfile : Profile
    {
        public RegisterProfile()
        {
            CreateMap<RegisterDTO, RegisterDto>().ReverseMap();
        }
    }
}
