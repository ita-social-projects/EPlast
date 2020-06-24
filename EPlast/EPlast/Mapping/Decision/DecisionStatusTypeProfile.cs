using AutoMapper;
using EPlast.BusinessLogicLayer.DTO;
using EPlast.Models;

namespace EPlast.Mapping
{
    public class DecisionStatusTypeProfile : Profile
    {
        public DecisionStatusTypeProfile()
        {
            CreateMap<Organization, OrganizationDTO>().ReverseMap();
        }
    }
}