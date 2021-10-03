using AutoMapper;
using EPlast.BLL.DTO.Club;
using DatabaseEntities = EPlast.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.BLL.Mapping.Club
{
    public class ClubReportPlastDegreesProfile : Profile
    {
        public ClubReportPlastDegreesProfile()
        {
            CreateMap<DatabaseEntities.ClubReportPlastDegrees, ClubReportPlastDegreesDTO>().ReverseMap();
        }
    }
}
