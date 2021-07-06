using AutoMapper;
using EPlast.BLL.DTO.GoverningBody;
using EPlast.WebApi.Models.GoverningBody;

namespace EPlast.WebApi.Mapping.GoverningBody
{
    public class GoverningBodyAdministrationProfile : Profile
    {
        public GoverningBodyAdministrationProfile()
        {
            CreateMap<GoverningBodyAdministrationViewModel, GoverningBodyAdministrationDTO>().ReverseMap();
        }
    }
}
