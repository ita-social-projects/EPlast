using AutoMapper;
using EPlast.BLL.DTO.Club;
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
