using AutoMapper;
using EPlast.BussinessLayer.DTO.Club;
using EPlast.ViewModels;

namespace EPlast.BussinessLayer.Mapping.Club
{
    public class ClubProfile : Profile
    {
        public ClubProfile()
        {
            CreateMap<DataAccess.Entities.Club, ClubDTO>().ReverseMap();
            CreateMap<ClubViewModel, ClubDTO>().ReverseMap();
        }
    }
}
