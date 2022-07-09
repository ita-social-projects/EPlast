using AutoMapper;
using EPlast.BLL.DTO.Club;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BLL.Mapping.Club
{
    public class ClubUserProfile : Profile
    {
        public ClubUserProfile()
        {
            CreateMap<DatabaseEntities.User, ClubUserDto>().ReverseMap();
        }
    }
}
