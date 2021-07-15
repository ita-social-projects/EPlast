﻿using AutoMapper;
using EPlast.BLL.DTO.Region;
using EPlast.DataAccess.Entities;

namespace EPlast.BLL.Mapping.Region
{
    class RegionFollowerProfile : Profile
    {
        public RegionFollowerProfile()
        {
            CreateMap<RegionFollowers, RegionFollowerDTO>().ReverseMap();
            CreateMap<RegionFollowerDTO, RegionFollowers>().ReverseMap();
        }
    }
}