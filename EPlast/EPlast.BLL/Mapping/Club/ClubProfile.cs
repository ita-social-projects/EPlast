﻿using AutoMapper;
using EPlast.BLL.DTO.Club;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BLL.Mapping.Club
{
    public class ClubProfile : Profile
    {
        public ClubProfile()
        {
            CreateMap<DatabaseEntities.Club, ClubDto>().ReverseMap();
            CreateMap<DatabaseEntities.Club, ClubObjectDto>().ReverseMap();
        }
    }
}