using AutoMapper;
using EPlast.BLL.DTO.GoverningBody.Announcement;
using EPlast.WebApi.Models.GoverningBody.Sector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.WebApi.Mapping.GoverningBody.Sector
{
    public class SectorAnnouncementsProfile: Profile
    {
        public SectorAnnouncementsProfile()
        {
            CreateMap<SectorAnnouncementsViewModel, GoverningBodyAnnouncementDTO>().ReverseMap();
            CreateMap<SectorViewModel, GoverningBodyAnnouncementDTO>().ReverseMap();
        }
    }
}
