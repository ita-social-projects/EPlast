using AutoMapper;
using EPlast.DataAccess.Entities.UserEntities;

namespace EPlast.BLL.Mapping.Precautions
{
    class PrecautionProfile : Profile
    {
        public PrecautionProfile()
        {
            CreateMap<Precaution, PrecautionDto>().ReverseMap();
        }
    }
}
