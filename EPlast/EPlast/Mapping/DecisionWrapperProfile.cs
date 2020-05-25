using AutoMapper;
using EPlast.BussinessLayer.DTO;
using EPlast.Models;

namespace EPlast.Mapping
{
    public class DecisionWrapperProfile : Profile
    {
        public DecisionWrapperProfile()
        {
            CreateMap<DecisionWrapper, DecisionWrapperDTO>()
                .ForMember(d => d.Decision, o => o.MapFrom(s => s.Decision))
                .ForMember(d => d.DecisionTargets, o => o.MapFrom(s => s.DecisionTargets))
                .ReverseMap();
        }
    }
}