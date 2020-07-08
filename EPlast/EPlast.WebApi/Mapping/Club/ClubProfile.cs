using AutoMapper;
using EPlast.BLL.DTO.Club;
using EPlast.WebApi.Models.Club;
using DataAccessClub = EPlast.DataAccess.Entities;

namespace EPlast.WebApi.Mapping.Club
{
    public class ClubProfile : Profile
    {
        public ClubProfile()
        {
            CreateMap<DataAccessClub.Club, ClubDTO>().ReverseMap();
            CreateMap<ClubDTO, ClubViewModel>().ReverseMap();
        }
    }
}
