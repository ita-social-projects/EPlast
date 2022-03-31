using AutoMapper;
using EPlast.BLL.DTO.GoverningBody.Announcement;
using EPlast.DataAccess.Entities.GoverningBody.Announcement;
using EPlast.DataAccess.Entities.GoverningBody.Sector;
using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.BLL.Mapping.GoverningBody.Sector
{
    public class SectorAnnouncementWithImages : Profile
    {
        public SectorAnnouncementWithImages()
        {
            CreateMap<SectorAnnouncementWithImagesDTO, SectorAnnouncement>().ReverseMap();
            CreateMap<SectorAnnouncementImageDTO, SectorAnnouncementImage>().ReverseMap();
        }
    }
}
