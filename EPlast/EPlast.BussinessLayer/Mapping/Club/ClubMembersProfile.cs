using AutoMapper;
using EPlast.BussinessLayer.DTO.Club;
using EPlast.DataAccess.Entities;

namespace EPlast.BussinessLayer.Mapping
{
    public class ClubMembersProfile : Profile
    {
        public ClubMembersProfile()
        {
            CreateMap<ClubMembers, ClubMembersDTO>().ReverseMap();
        }
    }
}