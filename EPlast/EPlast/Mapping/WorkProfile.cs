﻿using AutoMapper;
using EPlast.BussinessLayer.DTO;
using EPlast.DataAccess.Entities;
using EPlast.ViewModels.UserInformation.UserProfile;

namespace EPlast.Mapping
{
    public class WorkProfile:Profile
    {
        public WorkProfile()
        {
            CreateMap<Work, WorkDTO>().ReverseMap();
            CreateMap<WorkViewModel, WorkDTO>().ReverseMap();
        }
    }
}
