using AutoMapper;
using EPlast.BLL.DTO.GoverningBody;

namespace EPlast.BLL.Mapping.GoverningBody
{
    public class GoverningBodyUserProfile : Profile
    {
        public GoverningBodyUserProfile()
        {
            CreateMap<DataAccess.Entities.User, GoverningBodyUserDto>().ReverseMap();
        }
    }
}
