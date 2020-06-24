using AutoMapper;
using EPlast.BussinessLayer.DTO.Club;
using EPlast.ViewModels;

namespace EPlast.Mapping
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
