using AutoMapper;
using EPlast.BLL.DTO.EventUser;
using EPlast.DataAccess.Entities;
using EPlast.ViewModels.EventUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.Mapping.EventUser
{
    public class UserInfoProfile : Profile
    {
        public UserInfoProfile()
        {
            CreateMap<User, UserInfoDTO>().ReverseMap();
            CreateMap<UserInfoViewModel, UserInfoDTO>().ReverseMap();
        }

    }
}
