using AutoMapper;
using EPlast.BussinessLayer.DTO;
using EPlast.BussinessLayer.DTO.Club;
using EPlast.DataAccess.Entities;
using EPlast.ViewModels;
using EPlast.ViewModels.Club;

namespace EPlast.Mapping
{
    public class ClubProfile:Profile
    {
        public ClubProfile()
        {
            CreateMap<Club, ClubDTO>().ReverseMap();
            CreateMap<CLubViewModel, ClubDTO>().ReverseMap();
        }
    }
}
