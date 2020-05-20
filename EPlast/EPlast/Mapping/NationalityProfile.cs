using AutoMapper;
using EPlast.BussinessLayer.DTO;
using EPlast.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.Mapping
{
    public class NationalityProfile:Profile
    {
        public NationalityProfile()
        {
            CreateMap<Nationality, NationalityDTO>();
            CreateMap<NationalityDTO, Nationality>();
        }
    }
}
