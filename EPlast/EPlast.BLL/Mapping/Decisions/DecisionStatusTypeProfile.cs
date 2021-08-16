using AutoMapper;
using EPlast.BLL.DTO;

namespace EPlast.BLL.Mapping
{
    public class DecisionStatusTypeProfile : Profile
    {
        public DecisionStatusTypeProfile()
        {
            CreateMap<DataAccess.Entities.GoverningBody.Organization, GoverningBodyDTO>().ReverseMap();
        }
    }
}
