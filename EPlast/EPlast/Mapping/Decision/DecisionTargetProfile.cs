using AutoMapper;
using EPlast.BusinessLogicLayer.DTO;
using EPlast.Models;

namespace EPlast.Mapping
{
    public class DecisionTargetProfile : Profile
    {
        public DecisionTargetProfile()
        {
            CreateMap<DecisionTarget, DecisionTargetDTO>().ReverseMap();
        }
    }
}