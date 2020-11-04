using AutoMapper;
using EPlast.BLL.DTO.Club;
using EPlast.WebApi.Models.Club;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPlast.WebApi.Mapping.Club
{
    public class ClubAnnualReportProfile:Profile
    {
        public ClubAnnualReportProfile()
        {
            CreateMap<ClubAnnualReportDTO, ClubAnnualReportViewModel>().ReverseMap();
        }
    }
}
