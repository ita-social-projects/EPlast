using AutoMapper;
using EPlast.BLL.DTO;
using EPlast.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.BLL.Mapping.User
{
    public class UserTableProfile : Profile
    {
        public UserTableProfile()
        {
            CreateMap<UserTableObject, UserTableDTO>().ForPath(x => x.Gender.Name, d => d.MapFrom(y => y.Gender)).
                ForMember(x => x.UserProfileId, q => q.MapFrom(w => w.Index)).
                ForMember(u => u.UserPlastDegreeName, q => q.MapFrom(w => w.PlastDegree)).
                ForMember(u => u.UserRoles, q => q.MapFrom(w => w.Roles)).
                ReverseMap();
        }
    }
}

