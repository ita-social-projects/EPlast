using AutoMapper;
using EPlast.BussinessLayer.DTO;
using EPlast.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.Mapping
{
    public class DegreeProfile:Profile
    {
        public DegreeProfile()
        {
            CreateMap<Degree, DegreeDTO>();
            CreateMap<DegreeDTO, Degree>();
        }
    }
}
