﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.AboutBase;
using EPlast.WebApi.Models.AboutBase;

namespace EPlast.WebApi.Mapping.AboutBase
{
    public class AboutBaseSectionProfile: Profile
    {
        public AboutBaseSectionProfile()
        {
            CreateMap<SectionDto, AboutBaseSectionViewModel>().ReverseMap();
        }
    }
}
