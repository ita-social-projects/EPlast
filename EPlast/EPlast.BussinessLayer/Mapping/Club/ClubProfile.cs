using AutoMapper;
using EPlast.BussinessLayer.DTO.Club;
using DataAccessCity = EPlast.DataAccess.Entities;

namespace EPlast.BussinessLayer.Mapping.Club
{
    public class ClubProfile : Profile
    {
        public ClubProfile()
        {
            CreateMap<DataAccessCity.Club, ClubDTO>().ReverseMap();
        }
    }
}
