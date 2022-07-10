using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.Club;
using EPlast.WebApi.Models.Club;

namespace EPlast.WebApi.Mapping.Club
{
    public class ClubAnnualReportProfile:Profile
    {
        public ClubAnnualReportProfile()
        {
            CreateMap<ClubAnnualReportDto, ClubAnnualReportViewModel>().ReverseMap();
        }
    }
}
