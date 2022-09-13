using AutoMapper;
using EPlast.BLL.DTO.Club;
using EPlast.WebApi.Models.Club;

namespace EPlast.WebApi.Mapping.Club
{
    public class ClubProfileProfile : Profile
    {
        public ClubProfileProfile()
        {
            CreateMap<ClubProfileViewModel, ClubProfileDto>().ReverseMap();
        }
    }
}
