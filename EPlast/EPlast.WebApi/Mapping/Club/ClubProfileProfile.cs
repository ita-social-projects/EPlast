using AutoMapper;
using EPlast.BussinessLayer.DTO.Club;
using EPlast.WebApi.Models.Club;

namespace EPlast.WebApi.Mapping.Club
{
    public class ClubProfileProfile : Profile
    {
        public ClubProfileProfile()
        {
            CreateMap<ClubProfileViewModel, ClubProfileDTO>().ReverseMap();
        }
    }
}
