using AutoMapper;
using EPlast.BLL.DTO.GoverningBody.Sector;

namespace EPlast.BLL.Mapping.GoverningBody.Sector
{
    public class SectorUserProfile : Profile
    {
        public SectorUserProfile()
        {
            CreateMap<DataAccess.Entities.User, SectorUserDto>().ReverseMap();
        }
    }
}