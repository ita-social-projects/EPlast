using AutoMapper;
using EPlast.BussinessLayer.DTO;
using EPlast.Models;

namespace EPlast.Mapping
{
    public class DecisionStatusTypeProfile : Profile
    {
        public DecisionStatusTypeProfile()
        {
            CreateMap<Organization, OrganizationDTO>();
            CreateMap<OrganizationDTO, Organization>();
        }
    }
}