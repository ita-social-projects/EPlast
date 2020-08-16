using AutoMapper;
using EPlast.BLL.DTO.EducatorsStaff;
using EPlast.DataAccess.Entities.EducatorsStaff;
using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.BLL.Mapping.EducationsStaff
{
   public class KVsProfile:Profile
    {
        public KVsProfile()
        {
            CreateMap<KVs, KadrasDTO>().ReverseMap();
        }
        
    }
}
