﻿using AutoMapper;
using EPlast.BussinessLayer.DTO.City;
using EPlast.ViewModels.City;

namespace EPlast.Mapping.City
{
    public class CityProfileProfile : Profile
    {
        public CityProfileProfile()
        {
            CreateMap<CityProfileViewModel, CityProfileDTO>().ReverseMap();
        }
    }
}
