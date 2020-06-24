using AutoMapper;
using EPlast.Bussiness.DTO;
using EPlast.DataAccess.Entities;
using System.Collections.Generic;

namespace EPlast.Bussiness.Mapping
{
    public class OrganizationProfile : Profile
    {
        public OrganizationProfile()
        {
            CreateMap<Organization, OrganizationDTO>().ReverseMap();
        }
    }
}