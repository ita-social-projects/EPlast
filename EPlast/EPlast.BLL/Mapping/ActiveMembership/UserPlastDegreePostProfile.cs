using AutoMapper;
using EPlast.BLL.DTO.ActiveMembership;
using EPlast.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.BLL.Mapping.ActiveMembership
{
    public class UserPlastDegreePostProfile: Profile
    {
        public UserPlastDegreePostProfile()
        {
            CreateMap<UserPlastDegreePostDTO, UserPlastDegree>()
                .ForMember(upd => upd.PlastDegreeId, updp => updp.MapFrom(f => f.PlastDegree.Id))
                .ForMember(upd => upd.UserId, updp => updp.MapFrom(f => f.User.Id))
                .ReverseMap();
        }
    }
}
