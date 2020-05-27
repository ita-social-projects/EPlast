using AutoMapper;
using EPlast.BussinessLayer.DTO;
using EPlast.BussinessLayer.DTO.Club;
using EPlast.DataAccess.Entities;
using EPlast.ViewModels;
using EPlast.ViewModels.Club;

namespace EPlast.Mapping
{
    public class ClubMembersProfile : Profile
    {
        public ClubMembersProfile()
        {
            CreateMap<ClubMembers, ClubMembersDTO>().ReverseMap();
            CreateMap<ClubMembersViewModel, ClubMembersDTO>().ReverseMap();
        }
    }
}
