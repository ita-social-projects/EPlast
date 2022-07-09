using AutoMapper;
using EPlast.BLL.DTO.Club;
using EPlast.DataAccess.Entities;
using EPlast.WebApi.Models.Club;

namespace EPlast.WebApi.Mapping.Club
{
    public class ClubMembersProfile : Profile
    {
        public ClubMembersProfile()
        {
            CreateMap<ClubMembersDto, ClubMembersViewModel>().ReverseMap();
        }
    }
}