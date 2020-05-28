﻿using AutoMapper;
using EPlast.BussinessLayer.DTO;
using EPlast.DataAccess.Entities;
using EPlast.ViewModels;

namespace EPlast.Mapping
{
    public class ClubMembersProfile : Profile
    {
        public ClubMembersProfile()
        {
            CreateMap<ClubMembers, ClubMembersDTO>().ReverseMap();
            CreateMap<ClubMembersViewModel, ClubMembersDTO>().ReverseMap();
        }
    }
}