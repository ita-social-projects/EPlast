using AutoMapper;
using EPlast.BussinessLayer.DTO.Club;
using EPlast.DataAccess.Entities;
//using EPlast.ViewModels;

namespace EPlast.BussinessLayer.Mapping
{
    public class ClubProfile : Profile
    {
        public ClubProfile()
        {
            CreateMap<Club, ClubDTO>().ReverseMap();
            //CreateMap<ClubViewModel, ClubDTO>().ReverseMap();
        }
    }
}
