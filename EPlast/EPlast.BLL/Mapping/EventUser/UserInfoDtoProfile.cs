using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using EPlast.BLL.DTO.EventUser;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BLL.Mapping.EventUser
{
    class UserInfoDtoProfile : Profile
    {
        public UserInfoDtoProfile()
        {
            CreateMap<DatabaseEntities.User, UserInfoDto>().ReverseMap();
        }
    }
}
