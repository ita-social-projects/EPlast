using AutoMapper;
using EPlast.BLL.DTO.Club;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BLL.Mapping.Club
{
    public class ClubAdministrationProfile : Profile
    {
        public ClubAdministrationProfile()
        {
            CreateMap<DatabaseEntities.ClubAdministration, ClubAdministrationDto>().ReverseMap();
        }
    }
}
