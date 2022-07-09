using AutoMapper;
using EPlast.BLL.DTO.Club;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BLL.Mapping.Club
{
    public class ClubMembersProfile : Profile
    {
        public ClubMembersProfile()
        {
            CreateMap<DatabaseEntities.ClubMembers, ClubMembersDto>().ReverseMap();
        }
    }
}
