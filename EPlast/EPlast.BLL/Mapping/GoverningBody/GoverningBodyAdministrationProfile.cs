using AutoMapper;
using EPlast.BLL.DTO.GoverningBody;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BLL.Mapping.GoverningBody
{
    public class GoverningBodyAdministrationProfile : Profile
    {
        public GoverningBodyAdministrationProfile()
        {
            CreateMap<DatabaseEntities.GoverningBody.GoverningBodyAdministration, GoverningBodyAdministrationDTO>().ReverseMap();
        }
    }
}
