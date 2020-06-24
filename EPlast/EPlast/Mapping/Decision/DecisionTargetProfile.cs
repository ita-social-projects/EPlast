using AutoMapper;
using EPlast.BLL.DTO;
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