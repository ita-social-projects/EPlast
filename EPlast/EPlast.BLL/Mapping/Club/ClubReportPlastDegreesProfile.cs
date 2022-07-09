using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using EPlast.BLL.DTO.Club;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BLL.Mapping.Club
{
    public class ClubReportPlastDegreesProfile : Profile
    {
        public ClubReportPlastDegreesProfile()
        {
            CreateMap<DatabaseEntities.ClubReportPlastDegrees, ClubReportPlastDegreesDto>().ReverseMap();
        }
    }
}
