using AutoMapper;
using EPlast.BLL.DTO.GoverningBody;
using EPlast.WebApi.Models.GoverningBody;

namespace EPlast.WebApi.Mapping.GoverningBody
{
    public class GoverningBodyMembersProfile : Profile
    {
        public GoverningBodyMembersProfile()
        {
            CreateMap<GoverningBodyUserViewModel, GoverningBodyUserDto>().ReverseMap();
        }
    }
}
