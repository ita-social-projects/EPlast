﻿using AutoMapper;
using EPlast.BLL.DTO.GoverningBody.Announcement;
using EPlast.DataAccess.Entities.GoverningBody.Sector;
using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.BLL.Mapping.GoverningBody.Sector
{
    public class SectorAnnouncementUserProfile : Profile
    {
        public SectorAnnouncementUserProfile()
        {
            CreateMap<SectorAnnouncement, GoverningBodyAnnouncementUserDTO>().ReverseMap();
        }
    }
}
