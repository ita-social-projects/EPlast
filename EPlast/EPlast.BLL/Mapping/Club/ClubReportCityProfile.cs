﻿using AutoMapper;
using EPlast.BLL.DTO.Club;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BLL.Mapping.Club
{
    class ClubReportCityProfile : Profile
    {
        public ClubReportCityProfile()
        {
            CreateMap<DatabaseEntities.City, ClubReportCityDto>().ReverseMap();
        }
    }
}