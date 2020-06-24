using AutoMapper;
using EPlast.BLL.DTO;
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