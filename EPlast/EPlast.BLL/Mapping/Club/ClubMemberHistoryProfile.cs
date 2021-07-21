using AutoMapper;
using EPlast.BLL.DTO.Club;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BLL.Mapping.Club
{
    class ClubMemberHistoryProfile : Profile
    {
        public ClubMemberHistoryProfile()
        {
            CreateMap<DatabaseEntities.ClubMemberHistory, ClubMemberHistoryDTO>().ReverseMap();
        }
    }
}
