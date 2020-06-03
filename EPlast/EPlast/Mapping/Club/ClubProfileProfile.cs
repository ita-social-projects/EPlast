using AutoMapper;
using EPlast.BussinessLayer.DTO.Club;
using EPlast.ViewModels;

namespace EPlast.Mapping
{
    public class ClubProfileProfile : Profile
    {
        public ClubProfileProfile()
        {
            CreateMap<ClubProfileViewModel, ClubProfileDTO>().ReverseMap();
        }
    }
}
