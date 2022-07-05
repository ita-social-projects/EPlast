using AutoMapper;
using EPlast.BLL.DTO.Club;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BLL.Mapping.Club
{
    public class ClubLegalStatusProfile : Profile
    {
        public ClubLegalStatusProfile()
        {
            CreateMap<DatabaseEntities.ClubLegalStatus, ClubLegalStatusDto>().ReverseMap();
        }
    }
}
