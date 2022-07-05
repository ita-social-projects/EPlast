using AutoMapper;
using EPlast.BLL.DTO.Club;
using EPlast.WebApi.Models.Club;

namespace EPlast.WebApi.Mapping.Club
{
    public class ClubUserProfile:Profile
    {
        public ClubUserProfile()
        {
            CreateMap<ClubUserViewModel, ClubUserDto>().ReverseMap();

        }
    }
}
