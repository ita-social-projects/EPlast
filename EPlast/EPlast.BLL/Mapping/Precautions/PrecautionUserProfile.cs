using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using EPlast.BLL.DTO.PrecautionsDTO;

namespace EPlast.BLL.Mapping.Precautions
{
    public class PrecautionUserProfile : Profile
    {
        public PrecautionUserProfile()
        {
            CreateMap<DataAccess.Entities.User, PrecautionUserDTO>().ReverseMap();
        }
    }
}
