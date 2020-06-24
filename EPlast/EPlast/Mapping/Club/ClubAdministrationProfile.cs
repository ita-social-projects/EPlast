﻿using AutoMapper;
using EPlast.BusinessLogicLayer.DTO.Club;
using EPlast.DataAccess.Entities;
using EPlast.ViewModels;

namespace EPlast.Mapping
{
    public class ClubAdministrationProfile : Profile
    {
        public ClubAdministrationProfile()
        {
            CreateMap<ClubAdministration, ClubAdministrationDTO>().ReverseMap();
            CreateMap<ClubAdministrationViewModel, ClubAdministrationDTO>().ReverseMap();
        }
    }
}
