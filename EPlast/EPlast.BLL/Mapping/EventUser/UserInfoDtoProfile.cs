using AutoMapper;
using EPlast.BLL.DTO.EventUser;
using DatabaseEntities = EPlast.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.BLL.Mapping.EventUser
{
    class UserInfoDtoProfile : Profile
    {
        public UserInfoDtoProfile()
        {
            CreateMap<DatabaseEntities.User, UserInfoDTO>().ReverseMap();
        }
    }
}
