using AutoMapper;
using EPlast.BussinessLayer.DTO.Club;
using EPlast.DataAccess.Entities;
using EPlast.WebApi.Models.Club;

namespace EPlast.WebApi.Mapping.Club
{
    public class ClubMembersProfile : Profile
    {
        public ClubMembersProfile()
        {
            CreateMap<ClubMembers, ClubMembersDTO>().ReverseMap();
            CreateMap<ClubMembersDTO, ClubMembersViewModel>().ReverseMap();
        }
    }
}