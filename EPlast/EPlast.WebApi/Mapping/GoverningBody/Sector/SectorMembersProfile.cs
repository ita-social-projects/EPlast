using AutoMapper;
using EPlast.BLL.DTO.GoverningBody.Sector;
using EPlast.WebApi.Models.GoverningBody.Sector;

namespace EPlast.WebApi.Mapping.GoverningBody.Sector
{
    public class SectorMembersProfile : Profile
    {
        public SectorMembersProfile()
        {
            CreateMap<SectorUserViewModel, SectorUserDTO>().ReverseMap();
        }
    }
}