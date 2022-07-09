using AutoMapper;
using EPlast.DataAccess.Entities.UserEntities;

namespace EPlast.BLL.Mapping.Distinctions
{
    class DistinctionProfile : Profile
    {
        public DistinctionProfile()
        {
            CreateMap<Distinction, DistinctionDto>().ReverseMap();
        }
    }
}
