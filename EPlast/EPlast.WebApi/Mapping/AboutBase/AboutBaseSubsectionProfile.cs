using AutoMapper;
using EPlast.BLL.DTO.AboutBase;
using EPlast.WebApi.Models.AboutBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.WebApi.Mapping.AboutBase
{
    public class AboutBaseSubsectionProfile: Profile
    {
        public AboutBaseSubsectionProfile()
        {
            CreateMap<SubsectionDTO, AboutBaseSubsectionViewModel>().ReverseMap();
        }
    }
}
