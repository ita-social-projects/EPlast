using AutoMapper;
using EPlast.BussinessLayer.DTO;
using EPlast.Models;

namespace EPlast.Mapping
{
    public class DecisionProfile : Profile
    {
        public DecisionProfile()
        {
            CreateMap<Decision, DecisionDTO>()
                .ForMember(d => d.DecisionStatusType, o => o.MapFrom(s => s.DecisionStatusType))
                .ForMember(d => d.DecisionTarget, o => o.MapFrom(s => s.DecisionTarget))
                .ForMember(d => d.Organization, o => o.MapFrom(s => s.Organization))
                .ReverseMap();
        }
    }
}